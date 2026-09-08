using CRM.Application.DTOs.User;
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

        private readonly UserMapper _userMapper;

        public UserService(IUserRepository userRepository, IHasherService hasherService, UserMapper userMapper)
        {
            _userRepository = userRepository;
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
            var user = _userMapper.ToDomain(dto);

            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteAsync(int userId)
        {
            return await _userRepository.DeleteAsync(userId);
        }

        public async Task<UserDetailsDto?> GetByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return _userMapper.ToDetailsDto(user);
        }

        public async Task<List<UserDetailsDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _userMapper.ToDetailsDtos(users);
        }
    }
}
