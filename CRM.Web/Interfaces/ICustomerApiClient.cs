using CRM.Application.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Interfaces
{
    public interface ICustomerApiClient
    {
        public Task<List<CustomerDetailsDto>?> GetAllAsync();

        public Task<List<CustomerListDto>?> GetAllCardsAsync();

        public Task<CustomerDetailsDto?> GetByIdAsync(int id);

        public Task<bool> CreateAsync(CustomerCreateDto dto);

        public Task<int> CountCustomersAsync();
    }
}
