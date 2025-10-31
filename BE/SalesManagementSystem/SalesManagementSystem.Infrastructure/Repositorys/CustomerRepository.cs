using Microsoft.EntityFrameworkCore;
using SalesManagementSystem.Infrastructure.Models;
using SalesManagementSystem.Application.Interfaces.IRepositorys;
using SalesManagementSystem.Domain.Entity;

namespace SalesManagementSystem.Infrastructure.Repositorys;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerManagementContext _context;

    public CustomerRepository(CustomerManagementContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Lấy customer theo ID
    /// </summary>
    public async Task<Customer?> GetByIdAsync(ulong id)
    {
        return await _context.Customers
            .Include(c => c.CustomerType)
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);
    }

    /// <summary>
    /// Lấy tất cả customers
    /// </summary>
    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Include(c => c.CustomerType)
            .Where(c => c.DeletedAt == null)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Tạo customer mới
    /// </summary>
    public async Task<Customer> CreateAsync(Customer customer)
    {
        // Generate customer code
        customer.CustomerCode = await GenerateCustomerCode();
        customer.CreatedAt = DateTime.Now;
        customer.UpdatedAt = DateTime.Now;

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        // Load related data
        if (customer.CustomerTypeId.HasValue)
        {
            await _context.Entry(customer)
                .Reference(c => c.CustomerType)
                .LoadAsync();
        }

        return customer;
    }

    /// <summary>
    /// Update customer
    /// </summary>
    public async Task<bool> UpdateAsync(Customer customer)
    {
        var existingCustomer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == customer.Id && c.DeletedAt == null);

        if (existingCustomer == null)
            return false;

        // Update fields
        existingCustomer.FullName = customer.FullName;
        existingCustomer.Email = customer.Email;
        existingCustomer.TaxCode = customer.TaxCode;
        existingCustomer.ShippingAddress = customer.ShippingAddress;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.CustomerTypeId = customer.CustomerTypeId;
        existingCustomer.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Soft delete customer
    /// </summary>
    public async Task<bool> DeleteAsync(ulong id)
    {
        var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer == null)
            return false;

        customer.DeletedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Kiểm tra phone đã tồn tại
    /// </summary>
    public async Task<bool> IsPhoneExistAsync(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return await _context.Customers
            .AnyAsync(c => c.Phone == phone && c.DeletedAt == null);
    }

    /// <summary>
    /// Kiểm tra phone đã tồn tại (trừ customer hiện tại)
    /// </summary>
    public async Task<bool> IsPhoneExistExceptIdAsync(string phone, ulong exceptId)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return await _context.Customers
            .AnyAsync(c => c.Phone == phone && c.Id != exceptId && c.DeletedAt == null);
    }

    /// <summary>
    /// Generate mã khách hàng tự động
    /// Format: KH + yyyyMM + 6 số tăng dần
    /// Ví dụ: KH202510000001, KH202510000002
    /// </summary>
    private async Task<string> GenerateCustomerCode()
    {
        string prefix = "KH";
        string yearMonth = DateTime.Now.ToString("yyyyMM");
        string fullPrefix = prefix + yearMonth;

        var lastCustomer = await _context.Customers
            .Where(c => c.CustomerCode.StartsWith(fullPrefix))
            .OrderByDescending(c => c.CustomerCode)
            .FirstOrDefaultAsync();

        int nextNumber = 1;

        if (lastCustomer != null)
        {
            string lastCode = lastCustomer.CustomerCode;
            string lastNumberStr = lastCode.Substring(fullPrefix.Length);

            if (int.TryParse(lastNumberStr, out int lastNumber))
            {
                nextNumber = lastNumber + 1;
            }
        }

        string sequenceNumber = nextNumber.ToString("D6");
        return fullPrefix + sequenceNumber;
    }

    public async Task<(List<Customer> Items, int Total)> GetPagedAsync(int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var query = _context.Customers
            .Include(c => c.CustomerType)
            .Where(c => c.DeletedAt == null);

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

    public async Task<(List<Customer> Items, int Total)> SearchAsync(string? keyword, int page, int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var query = _context.Customers
            .Include(c => c.CustomerType)
            .Where(c => c.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string pattern = $"%{keyword.Trim()}%";
            query = query.Where(c =>
                (c.Email != null && EF.Functions.Like(c.Email, pattern)) ||
                (c.Phone != null && EF.Functions.Like(c.Phone, pattern))
            );
        }

        var total = await query.CountAsync();

        var items = await query
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize) //page = 2, pageSize = 10 → Skip(10), Take(10)
            .Take(pageSize)
            .ToListAsync();

        return (items, total);
    }

}