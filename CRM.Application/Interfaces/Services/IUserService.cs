using CRM.Application.DTOs.User;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<UserDetailsDto> AddAsync(UserCreateDto user);

        public Task<bool> UpdateAsync(UserUpdateDto user);

        public Task<bool> DeleteAsync(int userId);

        public Task<UserDetailsDto?> GetByIdAsync(int userId);

        public Task<int> CountAsync();

        public Task<List<UserDetailsDto>> GetAllAsync();
    }
}
