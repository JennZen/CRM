using CRM.Application.DTOs.Customer;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly CustomerMapper _customerMapper;

        public CustomerService(ICustomerRepository customerRepository, CustomerMapper customerMapper)
        {
            _customerRepository = customerRepository;
            _customerMapper = customerMapper;
        }
        
        public async Task<List<CustomerListDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return _customerMapper.ToListDtos(customers);
        }

        public async Task<CustomerDetailsDto?> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return null;

            return _customerMapper.ToDetailsDto(customer);
        }

        public async Task<int> CountAsync()
        {
            return await _customerRepository.CountAsync();
        }

        public async Task<CustomerDetailsDto> CreateAsync(CustomerCreateDto customer)
        {
            var domainCustomer = _customerMapper.ToDomain(customer);
            var createdCustomer = await _customerRepository.CreateAsync(domainCustomer);

            return _customerMapper.ToDetailsDto(createdCustomer);
        }

        public async Task<bool> UpdateAsync(CustomerUpdateDto customer)
        {
            var domainCustomer = _customerMapper.ToDomain(customer);
            return await _customerRepository.UpdateAsync(domainCustomer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _customerRepository.DeleteAsync(id);
        }
    }
}
