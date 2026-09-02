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
        public List<Request> GetAll();

        public Request? GetById(int id);

        //public List<Request> GetByUser(int userId);

        public bool Add(Request request);

        /*public bool Update(Request request);
        public bool Delete(Request request); 

        public bool ChangeStatus(Request request, Status status);
        public bool SetManager(Request request, User manager);*/
    }
}