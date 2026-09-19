using CRM.Application.Interfaces.Repositories;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using DevExpress.Data;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UnitOfWork _uow;

        private readonly UserMapper _mapper;

        public UserRepository(UnitOfWork uow, UserMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<int> CountAsync()
        {
            return await _uow.Query<UserDb>().CountAsync();
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = await _uow.Query<UserDb>().ToListAsync();

            return _mapper.ToDomains(users);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _uow.GetObjectByKeyAsync<UserDb>(id);
           
            return _mapper.ToDomain(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _uow.Query<UserDb>().Where(u => u.Email == email).FirstOrDefaultAsync();

            return _mapper.ToDomain(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _uow.GetObjectByKeyAsync<UserDb>(id);

            if (user == null) return false;

            await _uow.DeleteAsync(user);
            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task DeactivateAsync(int userId)
        {
            var userDb = await _uow.GetObjectByKeyAsync<UserDb>(userId);

            if (userDb == null) return;

            userDb.IsActive = false;
            await _uow.CommitChangesAsync();
        }

        public async Task<bool> AddAsync(User user)
        {
            if (user == null) return false;

            var existingUser = await _uow.Query<UserDb>().
                Where(u => u.Email == user.Email 
                        && u.FirstName == user.FirstName
                        && u.LastName == user.LastName).FirstOrDefaultAsync();

            if (existingUser != null) return false;

            var newUser = new UserDb(_uow)
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = user.PasswordHash
            };

            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            if (user == null) return false;

            var userDb = await _uow.GetObjectByKeyAsync<UserDb>(user.Id);

            if(userDb == null) return false;

            userDb.PasswordHash = user.PasswordHash;
            userDb.IsActive = user.IsActive;
            userDb.LastName = user.LastName;
            userDb.FirstName = user.FirstName;
            userDb.Role = user.Role;
            userDb.Email = user.Email;
            userDb.UpdatedAt = DateTime.Now;

            await _uow.CommitChangesAsync();
            return true;
        }
    }
}
