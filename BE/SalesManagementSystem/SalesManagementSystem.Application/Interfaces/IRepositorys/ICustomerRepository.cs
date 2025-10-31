using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManagementSystem.Domain.Entity;

namespace SalesManagementSystem.Application.Interfaces.IRepositorys
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(ulong id);
        Task<List<Customer>> GetAllAsync();
        Task<Customer> CreateAsync(Customer customer);
        Task<bool> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(ulong id);
        Task<bool> IsPhoneExistAsync(string phone);
        Task<bool> IsPhoneExistExceptIdAsync(string phone, ulong exceptId);
        Task<(List<Customer> Items, int Total)> GetPagedAsync(int page, int pageSize);
        Task<(List<Customer> Items, int Total)> SearchAsync(string? keyword, int page, int pageSize);
    }
}
