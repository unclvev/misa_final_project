using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManagementSystem.Application.Dtos.CustomerDto;
using SalesManagementSystem.Application.Dtos.CustomerDto.CustomerDto;
using SalesManagementSystem.Application.Dtos.Paging;

namespace SalesManagementSystem.Application.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task<List<CustomerRespone>> GetAllCustomer();

        Task<CustomerRespone> AddCustomer(CreateCustomerRequest createCustomerRequest);
        Task<CustomerRespone> UpdateCustomer(ulong id, UpdateCustomerRequest updateCustomerRequest);
        Task<bool> DeleteCustomer(ulong id);
        Task<CustomerRespone?> GetById(ulong id);
        Task<PagedResponse<List<CustomerRespone>>> GetCustomersPaged(int page, int pageSize);
        Task<PagedResponse<List<CustomerRespone>>> SearchCustomers(string? keyword, int page, int pageSize);
    }
}
