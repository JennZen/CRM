using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces.Repositories
{
    public interface ICustomerRepository
    {
        public List<Customer> GetAll();

        public Customer? GetById(int id);

        public bool Add(Customer customer);

        public bool Update(Customer customer);

        public bool Delete(Customer customer);
    }
}
