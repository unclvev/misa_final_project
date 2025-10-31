using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SalesManagementSystem.Application.Dtos.CustomerDto;
using SalesManagementSystem.Application.Dtos.CustomerDto.CustomerDto;
using SalesManagementSystem.Application.Interfaces.IRepositorys;
using SalesManagementSystem.Application.Interfaces.IServices;
using SalesManagementSystem.Domain.Entity;
using SalesManagementSystem.Application.Dtos.Paging;
using System.IO;
using System.Text;
using SalesManagementSystem.Application.Dtos;

namespace SalesManagementSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerRespone> AddCustomer(CreateCustomerRequest createCustomerRequest)
        {
            var entity = new Customer
            {
                FullName = createCustomerRequest.FullName,
                Email = createCustomerRequest.Email,
                TaxCode = createCustomerRequest.TaxCode,
                ShippingAddress = createCustomerRequest.ShippingAddress,
                Phone = createCustomerRequest.Phone,
                CustomerTypeId = createCustomerRequest.CustomerTypeId
            };

            var added = await _customerRepository.CreateAsync(entity);

            return new CustomerRespone
            {
                Id = added.Id,
                CustomerTypeId = added.CustomerTypeId,
                CustomerCode = added.CustomerCode,
                FullName = added.FullName,
                Email = added.Email,
                TaxCode = added.TaxCode,
                ShippingAddress = added.ShippingAddress,
                Phone = added.Phone,
                LastPurchaseDate = added.LastPurchaseDate,
                ProductsSummary = added.ProductsSummary,
                LatestProductName = added.LatestProductName,
                CreatedAt = added.CreatedAt,
                UpdatedAt = added.UpdatedAt,
                DeletedAt = added.DeletedAt,
                CustomerType = added.CustomerType != null ? new CustomerTypeDto
                {
                    Id = added.CustomerType.Id,
                    TypeName = added.CustomerType.TypeName,
                    Description = added.CustomerType.Description
                } : null
            };
        }

        public async Task<List<CustomerRespone>> GetAllCustomer()
        {
            var entities = await _customerRepository.GetAllAsync();
            return entities.Select(c => new CustomerRespone
            {
                Id = c.Id,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCode = c.CustomerCode,
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                LastPurchaseDate = c.LastPurchaseDate,
                ProductsSummary = c.ProductsSummary,
                LatestProductName = c.LatestProductName,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                DeletedAt = c.DeletedAt,
                CustomerType = c.CustomerType != null ? new CustomerTypeDto
                {
                    Id = c.CustomerType.Id,
                    TypeName = c.CustomerType.TypeName,
                    Description = c.CustomerType.Description
                } : null
            }).ToList();
        }

        public async Task<CustomerRespone> UpdateCustomer(ulong id, UpdateCustomerRequest updateCustomerRequest)
        {
            var entity = new Customer
            {
                Id = id,
                FullName = updateCustomerRequest.FullName,
                Email = updateCustomerRequest.Email,
                TaxCode = updateCustomerRequest.TaxCode,
                ShippingAddress = updateCustomerRequest.ShippingAddress,
                Phone = updateCustomerRequest.Phone,
                CustomerTypeId = updateCustomerRequest.CustomerTypeId
            };

            var ok = await _customerRepository.UpdateAsync(entity);
            if (!ok) return null;

            var refreshed = await _customerRepository.GetByIdAsync(id);
            if (refreshed == null) return null;

            return new CustomerRespone
            {
                Id = refreshed.Id,
                CustomerTypeId = refreshed.CustomerTypeId,
                CustomerCode = refreshed.CustomerCode,
                FullName = refreshed.FullName,
                Email = refreshed.Email,
                TaxCode = refreshed.TaxCode,
                ShippingAddress = refreshed.ShippingAddress,
                Phone = refreshed.Phone,
                LastPurchaseDate = refreshed.LastPurchaseDate,
                ProductsSummary = refreshed.ProductsSummary,
                LatestProductName = refreshed.LatestProductName,
                CreatedAt = refreshed.CreatedAt,
                UpdatedAt = refreshed.UpdatedAt,
                DeletedAt = refreshed.DeletedAt,
                CustomerType = refreshed.CustomerType != null ? new CustomerTypeDto
                {
                    Id = refreshed.CustomerType.Id,
                    TypeName = refreshed.CustomerType.TypeName,
                    Description = refreshed.CustomerType.Description
                } : null
            };
        }

        public Task<bool> DeleteCustomer(ulong id)
        {
            var result = _customerRepository.DeleteAsync(id);
            return result;
        }

        public async Task<CustomerRespone?> GetById(ulong id)
        {
            var c = await _customerRepository.GetByIdAsync(id);
            if (c == null) return null;
            return new CustomerRespone
            {
                Id = c.Id,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCode = c.CustomerCode,
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                LastPurchaseDate = c.LastPurchaseDate,
                ProductsSummary = c.ProductsSummary,
                LatestProductName = c.LatestProductName,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                DeletedAt = c.DeletedAt,
                CustomerType = c.CustomerType != null ? new CustomerTypeDto
                {
                    Id = c.CustomerType.Id,
                    TypeName = c.CustomerType.TypeName,
                    Description = c.CustomerType.Description
                } : null
            };
        }

        public async Task<PagedResponse<List<CustomerRespone>>> GetCustomersPaged(int page, int pageSize)
        {
            var (items, total) = await _customerRepository.GetPagedAsync(page, pageSize);

            var data = items.Select(c => new CustomerRespone
            {
                Id = c.Id,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCode = c.CustomerCode,
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                LastPurchaseDate = c.LastPurchaseDate,
                ProductsSummary = c.ProductsSummary,
                LatestProductName = c.LatestProductName,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                DeletedAt = c.DeletedAt,
                CustomerType = c.CustomerType != null ? new CustomerTypeDto
                {
                    Id = c.CustomerType.Id,
                    TypeName = c.CustomerType.TypeName,
                    Description = c.CustomerType.Description
                } : null
            }).ToList();

            return new PagedResponse<List<CustomerRespone>>
            {
                Data = data,
                Meta = new Meta { Page = page, PageSize = pageSize, Total = total },
                Error = null
            };
        }

        public async Task<PagedResponse<List<CustomerRespone>>> SearchCustomers(string? keyword, int page, int pageSize)
        {
            var (items, total) = await _customerRepository.SearchAsync(keyword, page, pageSize);

            var data = items.Select(c => new CustomerRespone
            {
                Id = c.Id,
                CustomerTypeId = c.CustomerTypeId,
                CustomerCode = c.CustomerCode,
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                LastPurchaseDate = c.LastPurchaseDate,
                ProductsSummary = c.ProductsSummary,
                LatestProductName = c.LatestProductName,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                DeletedAt = c.DeletedAt,
                CustomerType = c.CustomerType != null ? new CustomerTypeDto
                {
                    Id = c.CustomerType.Id,
                    TypeName = c.CustomerType.TypeName,
                    Description = c.CustomerType.Description
                } : null
            }).ToList();

            return new PagedResponse<List<CustomerRespone>>
            {
                Data = data,
                Meta = new Meta { Page = page, PageSize = pageSize, Total = total },
                Error = null
            };
        }

    }
}
