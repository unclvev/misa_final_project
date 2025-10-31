namespace SalesManagementSystem.Application.Interfaces.IServices
{
    public class ImportResult
    {
        public int Created { get; set; }
        public int Total { get; set; }
        public byte[]? ErrorFile { get; set; }
        public string? ErrorFileName { get; set; }
    }

    public interface ICsvService
    {
        Task<ImportResult> ImportCustomersAsync(System.IO.Stream fileStream, System.Threading.CancellationToken cancellationToken);
        Task<byte[]> ExportAllCustomersAsync();
    }
}


