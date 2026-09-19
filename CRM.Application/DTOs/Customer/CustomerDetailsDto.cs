using CRM.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Customer
{
    public class CustomerDetailsDto
    {
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ContactPerson { get; set; } = string.Empty;

        public string Telephone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Comment { get; set; } = string.Empty;

        public List<RequestMiniDto> Requests { get; set; }

        public int NumberOfRequests { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
