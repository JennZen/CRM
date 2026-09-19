using CRM.Application.DTOs.User;

namespace CRM.Web.Interfaces
{
    public interface IUserApiClient
    {
        public Task<List<UserDetailsDto>?> GetAllAsync();
        
        public Task<UserDetailsDto?> GetByIdAsync(int id);

        public Task<List<UserWithRequestCountDto>?> GetWithNumberOfRequestsAsync();

        public Task<bool> CreateAsync(UserCreateDto dto);

        public Task<bool> UpdateAsync(int id, UserUpdateDto dto);

        public Task<bool> DeactivateAsync(int id);

        public Task<int> CountUsersAsync();
    }
}
