using CRM.Application.DTOs.Customer;
using CRM.Application.DTOs.Request;
using CRM.Domain.Enums;

namespace CRM.Web.Interfaces
{
    public interface IRequestApiClient
    {
        public Task<List<RequestDetailsDto>?> GetAllAsync(Status? status = null);

        public Task<RequestDetailsDto?> GetByIdAsync(int id);

        public Task<bool> CreateAsync(RequestCreateDto dto);

        public Task<List<RequestDetailsDto>?> GetMyRequestsAsync(Status? status = null);

        public Task<List<RequestRecentDto>?> GetRecentRequestsByUserAsync();

        public Task<int> CountRequestsAsync(Status? status);

        public Task<bool> UpdateAsync(int id, RequestUpdateDto dto);

        public Task<bool> ChangeRequestStatusAsync(int id, Status status);
    }
}
