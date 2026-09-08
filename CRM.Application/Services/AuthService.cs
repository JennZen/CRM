using CRM.Application.DTOs.User;
using CRM.Application.Interfaces.Repositories;
using CRM.Application.Interfaces.Services;
using CRM.Application.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        private readonly ITokenService _tokenService;

        private readonly IHasherService _hasherService;

        private readonly UserMapper _userMapper;

        public AuthService(IUserRepository userRepository, ITokenService tokenService, IHasherService hasherService, UserMapper userMapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _hasherService = hasherService;
            _userMapper = userMapper;
        }

        public async Task<AuthResultDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null) return null;

            var isValid = _hasherService.VerifyPassword(dto.Password, user.PasswordHash);
            if(!isValid) return null;

            var token = _tokenService.GenerateToken(user);

            return new AuthResultDto
            {
                User = _userMapper.ToDetailsDto(user),
                Token = token,
                ExpiresAt = DateTime.Now.AddMinutes(60)
            };
        }
    }
}
