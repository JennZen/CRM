using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Models
{
    [Persistent("users")]
    public class UserDb : XPObject
    {
        public UserDb(Session session) : base(session) { }

        private string _firstName = string.Empty;
        [Size(75)]
        public string FirstName
        {
            get => _firstName;
            set => SetPropertyValue(nameof(FirstName), ref _firstName, value);
        }

        private string _lastName = string.Empty;
        [Size(75)]
        public string LastName
        {
            get => _lastName;
            set => SetPropertyValue(nameof(LastName), ref _lastName, value);
        }

        private string _email = string.Empty;
        [Size(100)]
        public string Email
        {
            get => _email;
            set => SetPropertyValue(nameof(Email), ref _email, value);
        }

        private string _passwordHash = string.Empty;
        [Size(SizeAttribute.Unlimited)]
        public string PasswordHash
        {
            get => _passwordHash;
            set => SetPropertyValue(nameof(PasswordHash), ref _passwordHash, value);
        }

        private string _role = "User";
        [Size(25)]
        public string Role
        {
            get => _role;
            set => SetPropertyValue(nameof(Role), ref _role, value);
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetPropertyValue(nameof(CreatedAt), ref _createdAt, value);
        }

        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetPropertyValue(nameof(UpdatedAt), ref _updatedAt, value);
        }
    }
}
