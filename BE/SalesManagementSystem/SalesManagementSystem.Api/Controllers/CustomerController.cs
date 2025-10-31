using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesManagementSystem.Application.Dtos.CustomerDto;
using SalesManagementSystem.Application.Dtos.CustomerDto.CustomerDto;
using SalesManagementSystem.Application.Interfaces.IServices;
using SalesManagementSystem.Application.Dtos.Paging;

namespace SalesManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpGet("GetAllCustomer")]
        public async Task<IActionResult> GetAllCustomer()
        {
            try
            {
                var customers = await _customerService.GetAllCustomer();
                return Ok(customers);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("GetCustomersPaged")]
        public async Task<IActionResult> GetCustomersPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var response = await _customerService.GetCustomersPaged(page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PagedResponse<List<CustomerRespone>>
                {
                    Data = new List<CustomerRespone>(),
                    Meta = new Meta { Page = page, PageSize = pageSize, Total = 0 },
                    Error = new { message = ex.Message, innerException = ex.InnerException?.Message }
                });
            }
        }


        [HttpGet("SearchCustomers")]
        public async Task<IActionResult> SearchCustomers([FromQuery] string? keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var response = await _customerService.SearchCustomers(keyword, page, pageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PagedResponse<List<CustomerRespone>>
                {
                    Data = new List<CustomerRespone>(),
                    Meta = new Meta { Page = page, PageSize = pageSize, Total = 0 },
                    Error = new { message = ex.Message, innerException = ex.InnerException?.Message }
                });
            }
        }


        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] CreateCustomerRequest createCustomerRequest)
        {
            try
            {
                var addedCustomer = await _customerService.AddCustomer(createCustomerRequest);
                return Ok(addedCustomer);
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        [HttpPut("UpdateCustomer/{id}")]
        public async Task<IActionResult> UpdateCustomer(ulong id, [FromBody] UpdateCustomerRequest updateCustomerRequest)
        {
            try
            {
                var updatedCustomer = await _customerService.UpdateCustomer(id, updateCustomerRequest);
                return Ok(updatedCustomer);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(ulong id)
        {
            try
            {
                var result = await _customerService.DeleteCustomer(id);
                if (result)
                {
                    return Ok(new { message = "Customer deleted successfully." });
                }
                else
                {
                    return NotFound(new { message = "Customer not found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while processing your request.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        
    }
}
