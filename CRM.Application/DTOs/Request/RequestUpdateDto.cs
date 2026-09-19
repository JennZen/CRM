using CRM.Domain.Entities;
using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Request
{
    public class RequestUpdateDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Priority Priority { get; set; }

        public int CustomerId { get; set; }

        public int? ManagerId { get; set; }
    }
}
