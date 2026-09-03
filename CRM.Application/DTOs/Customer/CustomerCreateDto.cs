using CRM.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Customer
{
    public class CustomerCreateDto
    {
        public string Name { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Telephone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;
    }
}
