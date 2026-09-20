using CRM.Application.DTOs.User;
using CRM.Application.Exceptions;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IHasherService _hasherService;

        private readonly IRequestRepository _requestRepository;

        private readonly UserMapper _userMapper;

        public UserService(IUserRepository userRepository, IRequestRepository requestRepository,
            IHasherService hasherService, UserMapper userMapper)
        {
            _userRepository = userRepository;
            _requestRepository = requestRepository;
            _hasherService = hasherService;
            _userMapper = userMapper;
        }

        public async Task<int> CountAsync()
        {
            return await _userRepository.CountAsync();
        }

        public async Task<UserDetailsDto> AddAsync(UserCreateDto dto)
        {
            var user = _userMapper.ToDomain(dto);
            user.PasswordHash = _hasherService.HashPassword(dto.Password);
            await _userRepository.AddAsync(user);

            return _userMapper.ToDetailsDto(user);
        }

        public async Task<bool> UpdateAsync(UserUpdateDto dto)
        {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException(nameof(User), dto.Id);

            var user = _userMapper.ToDomain(dto);
            user.Role = dto.Role;
            user.IsActive = existingUser.IsActive;

            var updated = await _userRepository.UpdateAsync(user);

            if (!updated)
                throw new NotFoundException(nameof(User), dto.Id);

            return true;
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            var deleted = await _userRepository.DeleteAsync(userId);

            if (!deleted)
                throw new NotFoundException(nameof(User), userId);

            return true;
        }

        public async Task<UserDetailsDto> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);

            return _userMapper.ToDetailsDto(user);
        }

        public async Task DeactivateAsync(int userId)
        {
            await _userRepository.DeactivateAsync(userId);
        }

        public async Task<List<UserDetailsDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _userMapper.ToDetailsDtos(users);
        }

        public async Task<List<UserSelectDto>> GetAllActiveAsync()
        {
            var users = await _userRepository.GetAllActiveAsync();

            return _userMapper.ToSelectDtos(users);
        }

        public async Task<List<UserWithRequestCountDto>> GetUsersWithNumberOfRequestsAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var result = new List<UserWithRequestCountDto>();

            foreach(var user in users)
            {
                result.Add(new UserWithRequestCountDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    NumberRequests = await _requestRepository.CountByUserAndStatusAsync(user.Id, null)
                });
            }

            return result;
        }
    }
}
