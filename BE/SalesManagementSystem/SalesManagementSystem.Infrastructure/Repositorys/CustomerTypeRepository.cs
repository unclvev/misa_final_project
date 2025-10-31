using Microsoft.EntityFrameworkCore;
using SalesManagementSystem.Application.Interfaces.IRepositorys;
using SalesManagementSystem.Domain.Entity;
using SalesManagementSystem.Infrastructure.Models;

namespace SalesManagementSystem.Infrastructure.Repositorys;

public class CustomerTypeRepository : ICustomerTypeRepository
{
    private readonly CustomerManagementContext _context;

    public CustomerTypeRepository(CustomerManagementContext context)
    {
        _context = context;
    }

    public async Task<CustomerType?> GetByIdAsync(ulong id)
    {
        return await _context.CustomerTypes.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<CustomerType?> GetByTypeNameAsync(string typeName)
    {
        return await _context.CustomerTypes.FirstOrDefaultAsync(t => t.TypeName == typeName);
    }
}


