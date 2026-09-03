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
        public Task<List<Customer>> GetAllAsync();

        public Task<Customer?> GetByIdAsync(int id);

        public Task<Customer> CreateAsync(Customer customer);

        public Task<bool> UpdateAsync(Customer customer);   

        public Task<bool> DeleteAsync(int id);
    }
}
