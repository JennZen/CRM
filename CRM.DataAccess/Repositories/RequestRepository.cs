using CRM.Application.Interfaces.Repositories;
using CRM.DataAccess.Mapping;
using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using CRM.Domain.Enums;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
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

        /*public List<Request> GetByUser(int userId)
        {
            return _uow.Query<RequestDb>().Where(r => r.Manager.Oid == userId).Select(r => new Request
            {
                Id = r.Oid,
                // Map other properties here
            }).ToList();
        }*/

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

            await _uow.SaveAsync(requestDb);
            await _uow.CommitChangesAsync();
            return true;
        }

    }
}
