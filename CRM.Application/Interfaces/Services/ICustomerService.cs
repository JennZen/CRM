using CRM.Application.DTOs.Customer;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Services
{
    public interface ICustomerService
    {
        public Task<List<CustomerDetailsDto>> GetAllAsync();

        public Task<List<CustomerListDto>> GetAllCardsAsync(int userId, string? search = null);

        public Task<CustomerDetailsDto?> GetByIdAsync(int id);

        public Task<int> CountAsync();

        public Task<CustomerDetailsDto> CreateAsync(CustomerCreateDto customer);

        public Task<bool> UpdateAsync(CustomerUpdateDto customer);

        public Task<bool> DeleteAsync(int id);

    }
}
