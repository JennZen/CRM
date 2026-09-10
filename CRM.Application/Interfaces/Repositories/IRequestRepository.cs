using CRM.Domain.Entities;
using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Repositories
{
    public interface IRequestRepository
    {
        public Task<List<Request>> GetAllAsync();

        public Task<Request?> GetByIdAsync(int id);

        public Task<List<Request>> GetByUserAsync(int userId);

        public Task<int> CountByUserAndStatusAsync(int userId, Status? status);

        public Task<bool> AddAsync(Request request);
        
        public Task<bool> UpdateAsync(Request request);

        public Task<bool> DeleteAsync(int requestId); 

        public Task<bool> ChangeStatusAsync(int requestId, Status status);

        public Task<bool> SetManagerAsync(int requestId, int managerId);
    }
}