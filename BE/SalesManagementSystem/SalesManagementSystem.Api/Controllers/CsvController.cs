using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManagementSystem.Application.Interfaces.IServices;

namespace SalesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CsvController : ControllerBase
    {
        private readonly ICsvService _csvService;

        public CsvController(ICsvService csvService)
        {
            _csvService = csvService;
        }

        [HttpPost("CustomersCsv")]
        [Consumes("multipart/form-data")]
        [Produces("application/json", "text/csv")]
        public async Task<IActionResult> ImportCustomersCsv(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File CSV không hợp lệ." });

            using var stream = file.OpenReadStream();
            var result = await _csvService.ImportCustomersAsync(stream, cancellationToken);

            if (result.ErrorFile != null)
            {
                var name = string.IsNullOrWhiteSpace(result.ErrorFileName) ? "import_errors.csv" : result.ErrorFileName;
                return File(result.ErrorFile, "text/csv; charset=utf-8", name);
            }

            return Ok(new { created = result.Created, total = result.Total });
        }

        [HttpGet("ExportCustomers")]
        public async Task<IActionResult> ExportCustomers()
        {
            var bytes = await _csvService.ExportAllCustomersAsync();
            var fileName = $"customers_{DateTime.UtcNow:yyyyMMdd_HHmmss}.csv";
            return File(bytes, "text/csv; charset=utf-8", fileName);
        }
    }
}
