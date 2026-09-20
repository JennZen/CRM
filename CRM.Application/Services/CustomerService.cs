using CRM.Application.DTOs.Customer;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using DevExpress.Data.Linq;
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

        private readonly IRequestRepository _requestRepository;

        private readonly CustomerMapper _customerMapper;

        private readonly RequestMapper _requestMapper;

        public CustomerService(ICustomerRepository customerRepository, IRequestRepository requestRepository, 
            CustomerMapper customerMapper, RequestMapper requestMapper)
        {
            _customerRepository = customerRepository;
            _requestRepository = requestRepository;
            _customerMapper = customerMapper;
            _requestMapper = requestMapper;
        }
        
        public async Task<List<CustomerDetailsDto>> GetAllAsync()
        {
            var customers = await _customerRepository.GetAllAsync();

            return _customerMapper.ToDetailsDtos(customers);
        }

        public async Task<List<CustomerSelectDto>> GetAllActiveAsync()
        {
            var customers = await _customerRepository.GetAllActiveAsync();

            return _customerMapper.ToSelectDtos(customers);
        }

        public async Task<List<CustomerListDto>> GetAllCardsAsync(string? search = null)
        {
            var customers = await _customerRepository.GetAllAsync(search);
            var dtos = _customerMapper.ToListDtos(customers);

            foreach(var dto in dtos)
            {
                dto.NumberOfRequests = await _requestRepository.CountByCustomerAndStatusAsync(dto.Id, null);
                dto.NumberOfActiveRequests = await _requestRepository.CountByCustomerAndStatusAsync(dto.Id, Status.Active);
            }

            return dtos;
        }

        public async Task<CustomerDetailsDto> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) throw new NotFoundException(nameof(Request), id);

            var dto = _customerMapper.ToDetailsDto(customer);
            
            var requests = await _requestRepository.GetByCustomerAsync(id);
            dto.Requests = requests.Select(r => _requestMapper.ToMiniDto(r)).ToList();

            return dto;
        }

        public async Task<int> CountAsync()
        {
            return await _customerRepository.CountAsync();
        }

        public async Task ArchiveAsync(int id)
        {
            await _customerRepository.ArchiveAsync(id);
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
            var updated = await _customerRepository.UpdateAsync(domainCustomer);

            if(!updated)
                throw new NotFoundException(nameof(Customer), customer.Id);

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted =  await _customerRepository.DeleteAsync(id);

            if (!deleted)
                throw new NotFoundException(nameof(Customer), id);

            return true;
        }
    }
}
