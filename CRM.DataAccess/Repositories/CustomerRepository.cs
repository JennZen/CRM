using CRM.Application.Interfaces.Repositories;
using CRM.DataAccess.Mapping;
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

        public async Task<List<Customer>> GetAllAsync()
        {
            var customers = await _uow.Query<CustomerDb>().ToListAsync();
            return _mapper.ToDomains(customers);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var customer = await _uow.GetObjectByKeyAsync<CustomerDb>(id);
            if (customer == null) return null;
            return _mapper.ToDomain(customer);
        }

        public async Task<int> CountAsync()
        {
            return await _uow.Query<CustomerDb>().CountAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingCustomer = await _uow.GetObjectByKeyAsync<CustomerDb>(id);
            if (existingCustomer == null) return false;

            await _uow.DeleteAsync(existingCustomer);
            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            if(customer == null) return false;

            var existingCustomer = await _uow.GetObjectByKeyAsync<CustomerDb>(customer.Id);
            if (existingCustomer == null) return false;

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Telephone = customer.Telephone;
            existingCustomer.ContactPerson = customer.ContactPerson;
            existingCustomer.Comment = customer.Comment;
            existingCustomer.UpdatedAt = DateTime.UtcNow;

            await _uow.CommitChangesAsync();
            return true;
        }

        public async Task<Customer?> CreateAsync(Customer customer)
        {
            var existingCustomer = await _uow.Query<CustomerDb>().
                FirstOrDefaultAsync(c => c.Name == customer.Name && c.Email == customer.Email 
                && c.Telephone == customer.Telephone);
            
            if (existingCustomer != null) return null;

            var customerDb = new CustomerDb(_uow)
            {
                Name = customer.Name,
                Email = customer.Email,
                Telephone = customer.Telephone,
                ContactPerson = customer.ContactPerson,
                Comment = customer.Comment,
                UpdatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
            };

            await _uow.CommitChangesAsync();
            return _mapper.ToDomain(customerDb);
        }
    }
}
