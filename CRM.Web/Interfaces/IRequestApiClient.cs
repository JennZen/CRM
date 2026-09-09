using CRM.Application.DTOs.Request;

namespace CRM.Web.Interfaces
{
    public interface IRequestApiClient
    {
        public Task<List<RequestDetailsDto>?> GetAllAsync();

        public Task<RequestDetailsDto?> GetByIdAsync(int id);

        public Task<bool> CreateAsync(RequestCreateDto dto);
    }
}
