using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Customer
{
    public class CustomerListDto
    {
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Telephone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int NumberOfRequests { get; set; } = 0;

        public int NumberOfActiveRequests { get; set; } = 0;

        public DateTime CreatedAt { get; set; }
    }
}
