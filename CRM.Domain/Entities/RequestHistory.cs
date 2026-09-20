using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class RequestHistory
    {
        public int Id { get; set; }

        public int RequestId { get; set; }

        public Request? Request { get; set; }

        public string Action { get; set; } = string.Empty;

        public string AuthorName { get; set; } = "System";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
