using CRM.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities
{
    public class Request
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Status Status { get; set; } = Status.New;

        public Priority Priority { get; set; } = Priority.Low;

        public Customer? Customer { get; set; }

        public int CustomerId { get; set; }

        public User? Manager { get; set; }

        public int ManagerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; }

        public DateTime FinishedAt { get; set; }
        
    }
}
