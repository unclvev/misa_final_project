using SalesManagementSystem.Domain.Entity;

namespace SalesManagementSystem.Application.Interfaces.IRepositorys
{
    public interface ICustomerTypeRepository
    {
        Task<CustomerType?> GetByIdAsync(ulong id);
        Task<CustomerType?> GetByTypeNameAsync(string typeName);
    }
}


