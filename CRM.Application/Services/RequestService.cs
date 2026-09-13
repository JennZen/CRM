using CRM.Application.DTOs.Request;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using CRM.Domain.Enums;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        private readonly RequestMapper _requestMapper;

        public RequestService(IRequestRepository requestRepository, RequestMapper requestMapper)
        {
            _requestRepository = requestRepository;
            _requestMapper = requestMapper;
        }

        public async Task<List<RequestListDto>> GetAllAsync()
        {
            var requests = await _requestRepository.GetAllAsync();

            return _requestMapper.ToListDtos(requests);
        }

        public async Task<RequestDetailsDto?> GetByIdAsync(int id)
        {
            var request = await _requestRepository.GetByIdAsync(id);
            return request is null ? null : _requestMapper.ToDetailsDto(request);
        }

        public async Task<List<RequestDetailsDto>> GetByUserAsync(int userId)
        {
            var requests = await _requestRepository.GetByUserAsync(userId);
            return _requestMapper.ToDetailsDtos(requests);
        }

        public async Task<List<RequestRecentDto>> GetRecentByUserAsync(int userId)
        {
            var requests = await _requestRepository.GetRecentByUserAsync(userId);
            return _requestMapper.ToRecentDtos(requests);
        }


        public async Task<int> CountByUserAndStatusAsync(int userId, Status? status)
        {
            return await _requestRepository.CountByUserAndStatusAsync(userId, status);
        }

        public async Task<RequestDetailsDto> AddAsync(RequestCreateDto request)
        {
            var domainRequest = _requestMapper.ToDomain(request);
            await _requestRepository.AddAsync(domainRequest);
            return _requestMapper.ToDetailsDto(domainRequest);
        }

        public async Task<bool> UpdateAsync(RequestUpdateDto request)
        {
            var domainRequest = _requestMapper.ToDomain(request);
            return await _requestRepository.UpdateAsync(domainRequest);
        }

        public async Task<bool> DeleteAsync(int requestId)
        {
            return await _requestRepository.DeleteAsync(requestId);
        }

        public async Task<bool> ChangeStatusAsync(int requestId, Status status)
        {
            return await _requestRepository.ChangeStatusAsync(requestId, status);
        }

        public async Task<bool> SetManagerAsync(int requestId, int managerId)
        {
            return await _requestRepository.SetManagerAsync(requestId, managerId);
        }
    }
}