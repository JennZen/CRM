using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Mapping
{
    [Mapper]
    public partial class RequestMapper
    {
        [MapProperty(nameof(RequestDb.Oid), nameof(Request.Id))]
        [MapProperty($"{nameof(RequestDb.Customer)}.{nameof(CustomerDb.Oid)}", nameof(Request.CustomerId))]
        [MapProperty($"{nameof(RequestDb.Manager)}.{nameof(UserDb.Oid)}", nameof(Request.ManagerId))]
        public partial Request ToDomain(RequestDb r);

        public partial List<Request> ToDomains(List<RequestDb> r);
    }
}
