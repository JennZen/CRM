using CRM.Application.Interfaces.Repositories;
using CRM.Application.Mapping;
using CRM.DataAccess.Models;
using CRM.Domain.Entities;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly UnitOfWork _uow;
        private readonly CustomerMapper _mapper;

        public CustomerRepository(UnitOfWork uow, CustomerMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public List<Customer> GetAll()
        {
            return _mapper.ToDomains(_uow.Query<CustomerDb>().ToList());
        }

        public Customer? GetById(int id)
        {
            var customer = _uow.GetObjectByKey<CustomerDb>(id);
            if (customer == null) return null;
            return _mapper.ToDomain(customer);
        }
        

        public bool Add(Customer customer)
        {
            var customerDb = new CustomerDb(_uow)
            {
                Name = customer.Name,
                ContactPerson = customer.ContactPerson,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                Comment = customer.Comment,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _uow.CommitChanges();
            return true;
        }

        public bool Update(Customer customer)
        {
            var customerDb = _uow.GetObjectByKey<CustomerDb>(customer.Id);
            if (customerDb == null) return false;
            customerDb.Name = customer.Name;
            customerDb.ContactPerson = customer.ContactPerson;
            customerDb.PhoneNumber = customer.PhoneNumber;
            customerDb.Email = customer.Email;
            customerDb.Comment = customer.Comment;
            customerDb.UpdatedAt = DateTime.Now;
            _uow.CommitChanges();
            return true;
        }

        public bool Delete(Customer customer)
        {
            var customerDb = _uow.GetObjectByKey<CustomerDb>(customer.Id);
            if (customerDb == null) return false;
            _uow.Delete(customerDb);
            _uow.CommitChanges();
            return true;
        }
    }
}
