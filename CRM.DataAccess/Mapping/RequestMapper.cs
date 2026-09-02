using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using DevExpress.Xpo;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Mapping
{
    [Mapper]
    public partial class RequestMapper
    {
        private readonly UnitOfWork _uow;

        public RequestMapper(UnitOfWork uow)
        {
            _uow = uow;
        }

        [ObjectFactory]
        private RequestDb CreateRequestDb() => new RequestDb(_uow);

        [MapProperty(nameof(Request.Id), nameof(RequestDb.Oid))]
        [MapperIgnoreSource(nameof(Request.Customer))]
        [MapperIgnoreSource(nameof(Request.Manager))] 
        [MapperIgnoreTarget(nameof(RequestDb.Customer))]
        [MapperIgnoreTarget(nameof(RequestDb.Manager))]
        public partial RequestDb ToDb(Request request);

        public partial List<RequestDb> ToDbs(List<Request> requests);

        [MapProperty(nameof(RequestDb.Oid), nameof(Request.Id))]
        [MapperIgnoreSource(nameof(Request.Customer))]
        [MapperIgnoreSource(nameof(Request.Manager))] 
        [MapperIgnoreTarget(nameof(RequestDb.Customer))]
        [MapperIgnoreTarget(nameof(RequestDb.Manager))]
        public partial Request ToDomain(RequestDb requestDb);

        public partial List<Request> ToDomains(List<RequestDb> requestDbs);
    }
}
