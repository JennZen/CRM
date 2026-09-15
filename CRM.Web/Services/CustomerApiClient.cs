using CRM.Application.DTOs.Customer;
using CRM.Web.Interfaces;
using System.Net.Http.Headers;

namespace CRM.Web.Services
{
    public class CustomerApiClient : ICustomerApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContentAccessor;

        public CustomerApiClient(IHttpClientFactory factory, IHttpContextAccessor httpContentAccessor)
        {
            _httpClient = factory.CreateClient("CRM.Api");
            _httpContentAccessor = httpContentAccessor;
        }

        private void AttachToken()
        {
            var token = _httpContentAccessor.HttpContext?.Session.GetString("JwtToken");
            if(!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<CustomerDetailsDto>?> GetAllAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/customer");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<CustomerDetailsDto>>();
        }

        public async Task<List<CustomerListDto>?> GetAllCardsAsync(string? search = null)
        {
            AttachToken();

            var url = "api/customer/cards";
            if (!string.IsNullOrWhiteSpace(search))
            {
                url += $"?search={Uri.EscapeDataString(search)}";
            }

            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<CustomerListDto>>();
        }

        public async Task<CustomerDetailsDto?> GetByIdAsync(int id)
        {
            AttachToken();

            var response = await _httpClient.GetAsync($"api/customer/{id}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<CustomerDetailsDto>();
        }

        public async Task<bool> CreateAsync(CustomerCreateDto dto)
        {
            AttachToken();

            var response = await _httpClient.PostAsJsonAsync("api/customer", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateAsync(int id, CustomerUpdateDto dto)
        {
            AttachToken();

            var response = await _httpClient.PutAsJsonAsync($"api/customer/{id}", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<int> CountCustomersAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/customer/count");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
