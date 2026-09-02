using CRM.Domain.Entities;
using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.DataAccess.Models;

namespace CRM.Application.Interfaces.Repositories
{
    public interface IRequestRepository
    {
        public List<RequestDb> GetAll();

        public RequestDb GetById(int id);

        //public List<RequestDb> GetByUser(int userId);

        public bool Add(RequestDb request);

        public bool Update(RequestDb request);

        public bool Delete(RequestDb request); 

        public bool ChangeStatus(RequestDb request, Status status);
        public bool SetManager(RequestDb request, User manager);
    }
}
