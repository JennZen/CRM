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

        //public List<Request> GetByUser(int userId);

        public Task<bool> AddAsync(Request request);
        /*public Task<bool> UpdateAsync(Request request);
        public Task<bool> DeleteAsync(Request request); 

        public Task<bool> ChangeStatusAsync(Request request, Status status);
        public Task<bool> SetManagerAsync(Request request, User manager);*/
    }
}