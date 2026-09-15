using CRM.Application.Interfaces.Repositories;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly UnitOfWork _uow;
        private readonly RequestMapper _mapper;

        public RequestRepository(UnitOfWork uow, RequestMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<List<Request>> GetAllAsync()
        {
            var requests = await _uow.Query<RequestDb>().ToListAsync();

            return _mapper.ToDomains(requests);
        }

        public async Task<Request?> GetByIdAsync(int id)
        {
            var request = await _uow.GetObjectByKeyAsync<RequestDb>(id);

            if (request == null) return null;

            return _mapper.ToDomain(request);
        }

        public async Task<List<Request>> GetByUserAsync(int userId)
        {
            var requests = await _uow.Query<RequestDb>().Where(r => r.Manager.Oid == userId).ToListAsync();

            return _mapper.ToDomains(requests);
        }

        public async Task<int> CountByUserAndStatusAsync(int userId, Status? status)
        {
            var query =  _uow.Query<RequestDb>().Where(r => r.Manager.Oid == userId);

            if(status.HasValue)
            {
                query = query.Where(r => r.Status == status);
            }

            return await query.CountAsync();
        }

        public async Task<List<Request>> GetRecentByUserAsync(int userId, int n = 10)
        {
            var requests = await _uow.Query<RequestDb>().
                        Where(r => r.Manager.Oid == userId).
                        OrderByDescending(r => r.UpdatedAt).
                        Take(n).
                        ToListAsync();

            return _mapper.ToDomains(requests);
        }

        public async Task<bool> AddAsync(Request request)
        {
            if (request == null) return false;

            var customerDb = await _uow.GetObjectByKeyAsync<CustomerDb>(request.CustomerId);
            if (customerDb == null) return false;

            var duplicate = await _uow.Query<RequestDb>()
                .Where(r => r.Customer.Oid == request.CustomerId
                         && r.Title == request.Title
                         && r.FinishedAt == null)
                .FirstOrDefaultAsync();

            if (duplicate != null) return false;

            UserDb? managerDb = null;
            if (request.ManagerId != 0)
            {
                managerDb = await _uow.GetObjectByKeyAsync<UserDb>(request.ManagerId);
                if (managerDb == null) return false;
            }

            var requestDb = new RequestDb(_uow)
            {
                Title = request.Title,
                Description = request.Description,
                Status = Status.New,
                Priority = request.Priority,
                Customer = customerDb,
                Manager = managerDb,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                FinishedAt = null
            };

            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task<bool> ChangeStatusAsync(int requestId, Status status)
        {
            var requestDb = await _uow.GetObjectByKeyAsync<RequestDb>(requestId);

            if (requestDb == null) return false;

            requestDb.Status = status;

            await _uow.CommitChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int requestId)
        {
            var requestDb = await _uow.GetObjectByKeyAsync<RequestDb>(requestId);

            if (requestDb == null) return false;

            await _uow.DeleteAsync(requestDb);

            await _uow.CommitChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(Request request)
        {
            if(request == null) return false;

            var requestDb = await _uow.GetObjectByKeyAsync<RequestDb>(request.Id);
            if (requestDb == null) return false;

            requestDb.UpdatedAt = DateTime.Now;
            requestDb.Title = request.Title;
            requestDb.Description = request.Description;
            requestDb.Status = request.Status;
            requestDb.Priority = request.Priority;

            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task<bool> SetManagerAsync(int requestId, int managerId)
        {
            var requestDb = await _uow.GetObjectByKeyAsync<RequestDb>(requestId);
            if (requestDb == null) return false;

            var managerDb = await _uow.GetObjectByKeyAsync<UserDb>(managerId);
            if (managerDb == null) return false;

            requestDb.Manager = managerDb;
            await _uow.CommitChangesAsync();
            return true;
        }
    }
}
