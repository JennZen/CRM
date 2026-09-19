using CRM.Domain.Entities;
using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Request
{
    public class RequestListDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public  Status Status { get; set; } 

        public Priority Priority { get; set; }

        public string Customer { get; set; } = string.Empty;

        public string Manager { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
