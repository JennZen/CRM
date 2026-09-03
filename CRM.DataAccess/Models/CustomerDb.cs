using CRM.Domain.Entities;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Models
{
    [Persistent("customers")]
    public class CustomerDb : XPObject
    {
        public CustomerDb(Session session) : base(session) { }

        private string _name = string.Empty;

        [Size(255)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        private string _contactPerson = string.Empty;
        [Size(255)]
        public string ContactPerson
        {
            get => _contactPerson;
            set => SetPropertyValue(nameof(ContactPerson), ref _contactPerson, value);
        }

        private string _telephone = string.Empty;
        [Size(50)]
        public string Telephone
        {
            get => _telephone;
            set => SetPropertyValue(nameof(Telephone), ref _telephone, value);
        }

        private string _email = string.Empty;
        [Size(255)]
        public string Email
        {
            get => _email;
            set => SetPropertyValue(nameof(Email), ref _email, value);
        }

        private string _comment = string.Empty;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get => _comment;
            set => SetPropertyValue(nameof(Comment), ref _comment, value);
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetPropertyValue(nameof(CreatedAt), ref _createdAt, value);
        }

        private DateTime _updatedAt = DateTime.Now;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetPropertyValue(nameof(UpdatedAt), ref _updatedAt, value);
        }

        [Association("Customer-Requests")]
        public XPCollection<RequestDb> Requests
        {
            get => GetCollection<RequestDb>(nameof(Requests));
        }
    }
}
