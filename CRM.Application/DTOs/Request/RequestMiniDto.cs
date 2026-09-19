using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Request
{
    public class RequestMiniDto
    {
        public string Title { get; set; } = string.Empty;

        public Status Status { get; set; }

        public string Manager { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
