using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs.Request
{
    public class RequestHistoryDto
    {
        public string Action { get; set; } = string.Empty;

        public string AuthorName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
