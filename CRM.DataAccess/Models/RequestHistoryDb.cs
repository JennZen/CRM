using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DataAccess.Models
{
    [Persistent("request_history")]
    [DeferredDeletion(false)]
    public class RequestHistoryDb : XPObject
    {
        public RequestHistoryDb(Session session) : base(session) { }

        private RequestDb _request;
        [Persistent("request_id")]
        [Association("Request-History")]
        public RequestDb Request
        {
            get => _request;
            set => SetPropertyValue(nameof(Request), ref _request, value);
        }

        private string _action = string.Empty;
        [Size(500)]
        public string Action
        {
            get => _action;
            set => SetPropertyValue(nameof(Action), ref _action, value);
        }

        private string _authorName = "System";
        [Size(255)]
        public string AuthorName
        {
            get => _authorName;
            set => SetPropertyValue(nameof(AuthorName), ref _authorName, value);
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetPropertyValue(nameof(CreatedAt), ref _createdAt, value);
        }
    }
}
