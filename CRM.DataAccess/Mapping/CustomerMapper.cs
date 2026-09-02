using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using DevExpress.Xpo;
using Riok.Mapperly.Abstractions;
using System.Collections.Generic;

namespace CRM.Application.Mapping
{
    [Mapper]
    public partial class CustomerMapper
    {
        private readonly UnitOfWork _uow;

        public CustomerMapper(UnitOfWork uow)
        {
            _uow = uow;
        }

        [ObjectFactory]
        private CustomerDb CreateCustomerDb() => new CustomerDb(_uow);

        [MapProperty(nameof(Customer.Id), nameof(CustomerDb.Oid))]
        [MapperIgnoreSource(nameof(Customer.Requests))]
        [MapperIgnoreTarget(nameof(CustomerDb.Requests))]
        public partial CustomerDb ToDb(Customer customer);

        public partial List<CustomerDb> ToDbs(List<Customer> customers);

        [MapProperty(nameof(CustomerDb.Oid), nameof(Customer.Id))]
        [MapperIgnoreSource(nameof(Customer.Requests))]
        [MapperIgnoreTarget(nameof(CustomerDb.Requests))]
        public partial Customer ToDomain(CustomerDb customerDb);

        public partial List<Customer> ToDomains(List<CustomerDb> customerDbs);
    }
}