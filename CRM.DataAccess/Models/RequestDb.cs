using CRM.Domain.Entities;
using CRM.Domain.Enums;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Models
{
    [Persistent("requests")]
    [DeferredDeletion(false)]
    public class RequestDb : XPObject
    {
        public RequestDb(Session session) : base(session) { }


        private string _title = string.Empty;
        [Size(255)]
        public string Title
        {
            get => _title;
            set => SetPropertyValue(nameof(Title), ref _title, value);
        }

        private string _description = string.Empty;
        [Size(SizeAttribute.Unlimited)]
        public string Description
        {
            get => _description;
            set => SetPropertyValue(nameof(Description), ref _description, value);
        }

        private Status _status = Status.New;
        public Status Status
        {
            get => _status;
            set => SetPropertyValue(nameof(Status), ref _status, value);
        }

        private Priority _priority = Priority.Low;
        public Priority Priority
        {
            get => _priority;
            set => SetPropertyValue(nameof(Priority), ref _priority, value);
        }


        private CustomerDb _customer;
        [Persistent("customer_id")]
        [Association("Customer-Requests")]
        public CustomerDb Customer
        {
            get => _customer;
            set => SetPropertyValue(nameof(Customer), ref _customer, value);
        }

        private UserDb _manager;
        [Persistent("manager_id")]
        [Association("Manager-Requests")]
        public UserDb Manager
        {
            get => _manager;
            set => SetPropertyValue(nameof(Manager), ref _manager, value);
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

        private DateTime? _finishedAt;
        public DateTime? FinishedAt
        {
            get => _finishedAt;
            set => SetPropertyValue(nameof(FinishedAt), ref _finishedAt, value);
        }
    }
}
