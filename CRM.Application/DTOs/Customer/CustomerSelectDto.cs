using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Customer
{
    public class CustomerSelectDto
    {
        public int Id { get; set; }

        public bool IsArchived { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
