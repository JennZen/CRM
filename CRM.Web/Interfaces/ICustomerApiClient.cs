using CRM.Application.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Interfaces
{
    public interface ICustomerApiClient
    {
        public Task<List<CustomerDetailsDto>?> GetAllAsync();

        public Task<List<CustomerSelectDto>?> GetAllActiveAsync();

        public Task<List<CustomerListDto>?> GetAllCardsAsync(string? search = null);

        public Task<CustomerDetailsDto?> GetByIdAsync(int id);

        public Task<bool> CreateAsync(CustomerCreateDto dto);

        public Task<bool> UpdateAsync(int id, CustomerUpdateDto dto);

        public Task<int> CountCustomersAsync();

        public Task<bool> ArchiveAsync(int id);
    }
}
