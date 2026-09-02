using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Services.Services
{
    public class RequestService
    {
        private readonly UnitOfWork _uow;

        public RequestService(UnitOfWork uow)
        {
            _uow = uow;
        }


    }
}
