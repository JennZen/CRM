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
                Customer = r.Customer == null ? null : new Customer
                {
                    Id = r.Customer.Oid,
                    Name = r.Customer.Name,
                    ContactPerson = r.Customer.ContactPerson,
                    PhoneNumber = r.Customer.PhoneNumber,
                    Email = r.Customer.Email,
                    Comment = r.Customer.Comment,
                    CreatedAt = r.Customer.CreatedAt,
                    UpdatedAt = r.Customer.UpdatedAt
                },
                //map ManagerDb
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                FinishedAt = r.FinishedAt
            }).ToList();
        }

        public Request? GetById(int id)
        {
            var request = _uow.GetObjectByKey<RequestDb>(id);
            if (request == null) return null;
            return new Request
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                Customer = new Customer
                {
                    Id = request.Customer.Oid,
                    Name = request.Customer.Name,
                    ContactPerson = request.Customer.ContactPerson,
                    PhoneNumber = request.Customer.PhoneNumber,
                    Email = request.Customer.Email,
                    Comment = request.Customer.Comment,
                    CreatedAt = request.Customer.CreatedAt,
                    UpdatedAt = request.Customer.UpdatedAt
                },
                //map ManagerDb
                CreatedAt = request.CreatedAt,
                UpdatedAt = request.UpdatedAt,
                FinishedAt = request.FinishedAt
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
            var requestDb = new RequestDb(_uow)
            {
                Oid = request.Id,
                
            };
            _uow.CommitChanges();
            return true;
        }

    }
}
