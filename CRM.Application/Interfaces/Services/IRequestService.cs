using CRM.Application.DTOs.Request;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Services
{
    public interface IRequestService
    {
        public Task<List<RequestListDto>> GetAllAsync();

        public Task<RequestDetailsDto?> GetByIdAsync(int id);

        public Task<List<RequestDetailsDto>> GetByUserAsync(int userId, Status? status = null);

        public Task<List<RequestRecentDto>> GetRecentByUserAsync(int userId);

        public Task<int> CountByUserAndStatusAsync(int userId, Status? status);

        public Task<RequestDetailsDto> AddAsync(RequestCreateDto request);

        public Task<bool> UpdateAsync(RequestUpdateDto request);

        public Task<bool> DeleteAsync(int requestId);

        public Task<bool> ChangeStatusAsync(int requestId, Status status);

        public Task<bool> SetManagerAsync(int requestId, int managerId);
    }
}
