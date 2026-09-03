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
    public partial class CustomerMapper
    {
        [MapperIgnoreTarget(nameof(Customer.Requests))]
        [MapProperty(nameof(CustomerDb.Oid), nameof(Customer.Id))]
        public partial Customer ToDomain(CustomerDb c);

        public partial List<Customer> ToDomains(List<CustomerDb> c);

    }
}
