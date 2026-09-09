using CRM.Application.DTOs.Request;
using CRM.Web.Interfaces;
using System.Net.Http.Headers;

namespace CRM.Web.Services
{
    public class RequestApiClient : IRequestApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _contextAccessor;

        public RequestApiClient(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = factory.CreateClient("CRM.Api");
            _contextAccessor = httpContextAccessor;
        }

        private void AttachToken()
        {
            var token = _contextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<RequestDetailsDto>?> GetAllAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/request");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<RequestDetailsDto>>();

        }

        public async Task<RequestDetailsDto?> GetByIdAsync(int id)
        {
            AttachToken();

            var response = await _httpClient.GetAsync($"api/request/{id}");

            if(!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<RequestDetailsDto>();
        }

        public async Task<bool> CreateAsync(RequestCreateDto dto)
        {
            AttachToken();

            var response = await _httpClient.PostAsJsonAsync("api/request", dto);

            return response.IsSuccessStatusCode;
        }

    }
}
