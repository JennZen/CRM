using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> AddAsync(User user);

        public Task<bool> UpdateAsync(User user);

        public Task<bool> DeleteAsync(int userId);

        public Task<User?> GetByIdAsync(int userId);

        public Task<int> CountAsync();

        public Task<List<User>> GetAllAsync();

        public Task<User?> GetByEmailAsync(string email);
    }
}
