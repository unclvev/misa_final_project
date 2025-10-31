using System.Text;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SalesManagementSystem.Application.Interfaces.IRepositorys;
using SalesManagementSystem.Application.Interfaces.IServices;
using SalesManagementSystem.Domain.Entity;

namespace SalesManagementSystem.Application.Services
{
    public class CsvService : ICsvService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerTypeRepository _customerTypeRepository;

        public CsvService(ICustomerRepository customerRepository, ICustomerTypeRepository customerTypeRepository)
        {
            _customerRepository = customerRepository;
            _customerTypeRepository = customerTypeRepository;
        }

        /// <summary>
        /// Import customers từ CSV file stream
        /// </summary>
        public async Task<ImportResult> ImportCustomersAsync(Stream csvStream, CancellationToken cancellationToken)
        {
            // Đọc CSV stream với UTF-8 encoding, tự động detect BOM (Byte Order Mark)
            using var textReader = new StreamReader(csvStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true, leaveOpen: true);

            // Cấu hình CsvHelper để parse CSV
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,              // CSV có dòng header
                IgnoreBlankLines = true,             // Bỏ qua dòng trống
                TrimOptions = TrimOptions.Trim,      // Tự động trim whitespace
                BadDataFound = null,                 // Không throw exception khi gặp bad data
                MissingFieldFound = null,            // Không throw exception khi thiếu field
                HeaderValidated = null,              // Không validate header tự động
            };

            // Khởi tạo CsvReader với config
            using var csv = new CsvReader(textReader, config);

            // Đăng ký ClassMap để map columns trong CSV với properties của CustomerCsvRow
            csv.Context.RegisterClassMap<CustomerCsvRowMap>();

            var errors = new List<(int Row, string Error)>();
            var seenPhones = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // Track duplicate phones trong file
            int created = 0;
            int total = 0;

            try
            {
                // Đọc và validate header trước khi parse data
                await csv.ReadAsync();
                csv.ReadHeader();

                var headers = csv.HeaderRecord ?? Array.Empty<string>();
                var required = new[] { "FullName", "Phone", "Email", "Address", "CustomerTypeId" };

                // Kiểm tra các cột required có tồn tại không
                if (!required.All(r => headers.Contains(r, StringComparer.OrdinalIgnoreCase)))
                {
                    var missing = required.Where(r => !headers.Contains(r, StringComparer.OrdinalIgnoreCase));
                    return new ImportResult
                    {
                        Created = 0,
                        Total = 0,
                        ErrorFile = BuildErrorCsv(new[] { (1, $"Missing required headers: {string.Join(", ", missing)}") }),
                        ErrorFileName = BuildErrorFileName()
                    };
                }

                int rowNumber = 1; // Bắt đầu từ 1 vì dòng 1 là header

                // Đọc từng dòng CSV
                while (await csv.ReadAsync())
                {
                    rowNumber++;
                    cancellationToken.ThrowIfCancellationRequested(); // Check nếu user cancel request
                    total++;

                    CustomerCsvRow r;
                    try
                    {
                        // Parse dòng hiện tại thành CustomerCsvRow object
                        // CsvHelper tự động map columns với properties dựa vào CustomerCsvRowMap
                        r = csv.GetRecord<CustomerCsvRow>()!;
                    }
                    catch (Exception ex)
                    {
                        errors.Add((rowNumber, $"Parse error: {ex.Message}"));
                        continue;
                    }

                    // Validate từng field
                    var rowErrors = new List<string>();
                    if (string.IsNullOrWhiteSpace(r.FullName)) rowErrors.Add("FullName required");
                    if (string.IsNullOrWhiteSpace(r.Phone)) rowErrors.Add("Phone required");
                    if (string.IsNullOrWhiteSpace(r.Email)) rowErrors.Add("Email required");
                    if (string.IsNullOrWhiteSpace(r.Address)) rowErrors.Add("Address required");
                    if (r.CustomerTypeId == null || r.CustomerTypeId == 0) rowErrors.Add("CustomerTypeId required");

                    // Validate email format
                    if (!string.IsNullOrWhiteSpace(r.Email) && !IsValidEmail(r.Email))
                        rowErrors.Add("Email invalid");

                    if (!string.IsNullOrWhiteSpace(r.Phone))
                    {
                        // Kiểm tra duplicate phone trong file CSV
                        if (!seenPhones.Add(r.Phone))
                            rowErrors.Add("Duplicate phone in file");

                        // Kiểm tra phone đã tồn tại trong database chưa
                        if (await _customerRepository.IsPhoneExistAsync(r.Phone))
                            rowErrors.Add("Phone already exists in database");
                    }

                    // Kiểm tra CustomerType có tồn tại trong database không
                    CustomerType? type = null;
                    if (r.CustomerTypeId != null && r.CustomerTypeId > 0)
                    {
                        // Query database để lấy CustomerType
                        type = await _customerTypeRepository.GetByIdAsync((ulong)r.CustomerTypeId.Value);
                        if (type == null) rowErrors.Add("CustomerTypeId not found");
                    }

                    // Nếu có lỗi validation, thêm vào error list và skip dòng này
                    if (rowErrors.Count > 0)
                    {
                        errors.Add((rowNumber, string.Join("; ", rowErrors)));
                        continue;
                    }

                    // Tạo Customer entity từ CSV row
                    var entity = new Customer
                    {
                        FullName = r.FullName!,
                        Phone = r.Phone!,
                        Email = r.Email!,
                        ShippingAddress = r.Address!,
                        CustomerTypeId = type!.Id
                    };

                    // Lưu vào database
                    await _customerRepository.CreateAsync(entity);
                    created++;
                }
            }
            catch (Exception ex)
            {
                // Catch global exception (file corrupt, database down, etc.)
                return new ImportResult
                {
                    Created = 0,
                    Total = 0,
                    ErrorFile = BuildErrorCsv(new[] { (1, $"Error: {ex.Message}") }),
                    ErrorFileName = BuildErrorFileName()
                };
            }

            // Nếu có errors, trả về file CSV chứa các dòng bị lỗi
            if (errors.Count > 0)
            {
                return new ImportResult
                {
                    Created = created,
                    Total = total,
                    ErrorFile = BuildErrorCsv(errors),
                    ErrorFileName = BuildErrorFileName()
                };
            }

            // Success: trả về số lượng created và total
            return new ImportResult { Created = created, Total = total };
        }

        /// <summary>
        /// Export tất cả customers thành CSV file
        /// </summary>
        public async Task<byte[]> ExportAllCustomersAsync()
        {
            // Lấy tất cả customers từ database
            var customers = await _customerRepository.GetAllAsync();

            var sb = new StringBuilder();
            // Tạo header row
            sb.AppendLine("FullName,Email,Phone,TaxCode,Address,CustomerTypeId,CustomerTypeName,CustomerCode,CreatedAt");

            // Tạo data rows
            foreach (var c in customers)
            {
                var typeName = c.CustomerType != null ? c.CustomerType.TypeName : string.Empty;
                var createdAt = c.CreatedAt.HasValue ? c.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;

                // Join các fields với comma, escape đúng format CSV
                sb.AppendLine(string.Join(',', new string[]
                {
                    EscapeCsv(c.FullName),
                    EscapeCsv(c.Email ?? string.Empty),
                    EscapeCsv(c.Phone ?? string.Empty),
                    EscapeCsv(c.TaxCode ?? string.Empty),
                    EscapeCsv(c.ShippingAddress ?? string.Empty),
                    EscapeCsv(c.CustomerTypeId?.ToString() ?? string.Empty),
                    EscapeCsv(typeName),
                    EscapeCsv(c.CustomerCode ?? string.Empty),
                    EscapeCsv(createdAt)
                }));
            }

            // Convert string sang byte array với UTF-8 encoding
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Validate email format bằng .NET MailAddress class
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        /// <summary>
        /// DTO class để map CSV row
        /// </summary>
        private sealed class CustomerCsvRow
        {
            public string? FullName { get; set; }
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public string? Address { get; set; }
            public int? CustomerTypeId { get; set; }
        }

        /// <summary>
        /// CsvHelper ClassMap để mapping CSV columns với properties
        /// Định nghĩa rõ ràng column name nào map với property nào
        /// </summary>
        private sealed class CustomerCsvRowMap : ClassMap<CustomerCsvRow>
        {
            public CustomerCsvRowMap()
            {
                Map(m => m.FullName).Name("FullName");           // Column "FullName" -> FullName property
                Map(m => m.Phone).Name("Phone");                 // Column "Phone" -> Phone property
                Map(m => m.Email).Name("Email");                 // Column "Email" -> Email property
                Map(m => m.Address).Name("Address");             // Column "Address" -> Address property
                Map(m => m.CustomerTypeId).Name("CustomerTypeId"); // Column "CustomerTypeId" -> CustomerTypeId property
            }
        }

        /// <summary>
        /// Build error CSV file từ list errors
        /// Format: Row,Error
        /// </summary>
        private static byte[] BuildErrorCsv(IEnumerable<(int Row, string Error)> errors)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Row,Error");
            foreach (var e in errors)
            {
                // Escape double quotes trong error message (CSV standard)
                sb.AppendLine($"{e.Row},\"{e.Error.Replace("\"", "\"\"")}\"");
            }
            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        /// <summary>
        /// Tạo tên file error với timestamp
        /// Format: import_errors_YYYYMMDD_HHMMSS.csv
        /// </summary>
        private static string BuildErrorFileName()
        {
            return $"import_errors_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
        }

        /// <summary>
        /// Escape CSV value theo RFC 4180 standard
        /// Nếu value chứa comma, quotes, newline -> wrap trong quotes và escape quotes
        /// </summary>
        private static string EscapeCsv(string value)
        {
            if (value.Contains('"') || value.Contains(',') || value.Contains('\n') || value.Contains('\r'))
            {
                // Wrap trong quotes và escape internal quotes bằng double quotes
                return '"' + value.Replace("\"", "\"\"") + '"';
            }
            return value;
        }
    }
}