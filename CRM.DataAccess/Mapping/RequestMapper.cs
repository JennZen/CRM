using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CRM.DataAccess.Mapping
{
    [Mapper]
    public partial class RequestMapper
    {
        public Request ToDomain(RequestDb r)
        {
            return new Request
            {
                Id = r.Oid,
                Title = r.Title,
                Description = r.Description,
                Status = r.Status,
                Priority = r.Priority,

                CustomerId = r.Customer?.Oid ?? 0,
                Customer = r.Customer == null
                    ? null
                    : new Customer
                    {
                        Id = r.Customer.Oid,
                        Name = r.Customer.Name,
                        ContactPerson = r.Customer.ContactPerson,
                        Telephone = r.Customer.Telephone,
                        Email = r.Customer.Email,
                        Comment = r.Customer.Comment,
                        CreatedAt = r.Customer.CreatedAt,
                        UpdatedAt = r.Customer.UpdatedAt
                    },

                ManagerId = r.Manager?.Oid ?? 0,
                Manager = r.Manager == null
                    ? null
                    : new User
                    {
                        Id = r.Manager.Oid,
                        FirstName = r.Manager.FirstName,
                        LastName = r.Manager.LastName,
                        Email = r.Manager.Email,
                        PasswordHash = r.Manager.PasswordHash,
                        Role = r.Manager.Role,
                        CreatedAt = r.Manager.CreatedAt,
                        UpdatedAt = r.Manager.UpdatedAt
                    },

                History = r.History
                    .OrderByDescending(h => h.CreatedAt)
                    .Select(ToDomain)
                    .ToList(),

                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                FinishedAt = r.FinishedAt
            };
        }

        public RequestHistory ToDomain(RequestHistoryDb h)
        {
            return new RequestHistory
            {
                Id = h.Oid,
                RequestId = h.Request?.Oid ?? 0,
                Action = h.Action,
                AuthorName = h.AuthorName,
                CreatedAt = h.CreatedAt
            };
        }

        public List<Request> ToDomains(List<RequestDb> requests)
        {
            return requests.Select(ToDomain).ToList();
        }
    }
}