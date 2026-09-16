using CRM.Application.DTOs.User;
using CRM.Web.Interfaces;
using System.Net.Http.Headers;

namespace CRM.Web.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserApiClient(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = factory.CreateClient("CRM.Api");
            _httpContextAccessor = httpContextAccessor;
        }

        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<UserDetailsDto>?> GetAllAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/user");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<UserDetailsDto>>();
        }

        public async Task<UserDetailsDto?> GetByIdAsync(int id)
        {
            AttachToken();

            var response = await _httpClient.GetAsync($"api/user/{id}");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<UserDetailsDto>();
        }

        public async Task<List<UserWithRequestCountDto>?> GetWithNumberOfRequestsAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync($"api/user/with-request-count");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<List<UserWithRequestCountDto>>();
        }

        public async Task<bool> CreateAsync(UserCreateDto dto)
        {
            AttachToken();

            var response = await _httpClient.PostAsJsonAsync("api/user", dto);
            return response.IsSuccessStatusCode;
        }

        public async Task<int> CountUsersAsync()
        {
            AttachToken();

            var response = await _httpClient.GetAsync("api/user/count");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<int>(); 
        }
    }
}
