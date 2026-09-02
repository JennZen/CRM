using CRM.Application.Interfaces.Repositories;
using CRM.Domain.Entities;
using DevExpress.Xpo;
using CRM.DataAccess.Models;
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

        public RequestRepository(UnitOfWork uow)
        {
            _uow = uow;
        }

        public List<Request> GetAll()
        {
            return _uow.Query<RequestDb>().Select(r => new Request
            {
                Id = r.Oid,
                Title = r.Title,
                Description = r.Description,
                Status = r.Status,
                Priority = r.Priority,
                Customer

                // Map other properties here
            }).ToList();
        }

        public Request GetById(int id)
        {
            var request = _uow.GetObjectByKey<RequestDb>(id);
            if (request == null) return null;
            return new Request
            {
                Id = request.Oid,
                // Map other properties here
            };
        }

        /*public List<Request> GetByUser(int userId)
        {
            return _uow.Query<RequestDb>().Where(r => r.Manager.Oid == userId).Select(r => new Request
            {
                Id = r.Oid,
                // Map other properties here
            }).ToList();
        }*/

        public bool Add(Request request)
        {
            var requestDb = new RequestDb
            {
                Oid = request.Id,
                // Map other properties here
            };
            _uow.Save(requestDb);
            _uow.CommitChanges();
            return true;
        }

    }
}
