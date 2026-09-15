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
        public Task<List<Customer>> GetAllAsync(string? search = null);

        public Task<Customer?> GetByIdAsync(int id);

        public Task<int> CountAsync();

        public Task<Customer> CreateAsync(Customer customer);

        public Task<bool> UpdateAsync(Customer customer);   

        public Task<bool> DeleteAsync(int id);
    }
}
