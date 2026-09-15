using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Request
{
    public class RequestRecentDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public string Manager { get; set; } = string.Empty;

        public string Customer { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
