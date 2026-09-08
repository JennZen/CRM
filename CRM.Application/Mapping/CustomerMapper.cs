using CRM.Application.DTOs.Customer;
using CRM.Domain.Entities;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Mapping
{
    [Mapper]
    public partial class CustomerMapper
    {
        public partial CustomerDetailsDto  ToDetailsDto (Customer c);

        public partial List<CustomerDetailsDto> ToDetailsDtos(List<Customer> c);

        public partial CustomerListDto ToListDto(Customer c);
        
        public partial List<CustomerListDto> ToListDtos(List<Customer> c);

        public partial Customer ToDomain(CustomerUpdateDto c);

        public partial Customer ToDomain(CustomerCreateDto c);
    }
}
