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
                FullName = added.FullName,
                Email = added.Email,
                TaxCode = added.TaxCode,
                ShippingAddress = added.ShippingAddress,
                Phone = added.Phone,
                CustomerTypeId = added.CustomerTypeId
            };
        }

        public async Task<List<CustomerRespone>> GetAllCustomer()
        {
            var entities = await _customerRepository.GetAllAsync();
            return entities.Select(c => new CustomerRespone
            {
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                CustomerTypeId = c.CustomerTypeId
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
                FullName = refreshed.FullName,
                Email = refreshed.Email,
                TaxCode = refreshed.TaxCode,
                ShippingAddress = refreshed.ShippingAddress,
                Phone = refreshed.Phone,
                CustomerTypeId = refreshed.CustomerTypeId
            };
        }

        public Task<bool> DeleteCustomer(ulong id)
        {
            var result = _customerRepository.DeleteAsync(id);
            return result;
        }

        public async Task<PagedResponse<List<CustomerRespone>>> GetCustomersPaged(int page, int pageSize)
        {
            var (items, total) = await _customerRepository.GetPagedAsync(page, pageSize);

            var data = items.Select(c => new CustomerRespone
            {
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                CustomerTypeId = c.CustomerTypeId
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
                FullName = c.FullName,
                Email = c.Email,
                TaxCode = c.TaxCode,
                ShippingAddress = c.ShippingAddress,
                Phone = c.Phone,
                CustomerTypeId = c.CustomerTypeId
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
