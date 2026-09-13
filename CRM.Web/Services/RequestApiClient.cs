using CRM.Application.DTOs.Request;
using CRM.Domain.Enums;
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

        public async Task<List<RequestDetailsDto>?> GetMyRequestsAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/request/my");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<RequestDetailsDto>>();
        }

        public async Task<List<RequestRecentDto>?> GetRecentRequestsByUserAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/request/recent");

            if(!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<RequestRecentDto>>();
        }

        public async Task<bool> CreateAsync(RequestCreateDto dto)
        {
            AttachToken();

            var response = await _httpClient.PostAsJsonAsync("api/request", dto);

            return response.IsSuccessStatusCode;
        }

        public async Task<int> CountRequestsAsync(Status? status)
        {
            AttachToken();

            var url = "api/request/my/count";

            if(status.HasValue)
            {
                url += $"?status={status.Value}";
            }

            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
