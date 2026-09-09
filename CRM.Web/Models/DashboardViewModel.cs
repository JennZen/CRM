namespace CRM.Web.Models
{
    public class DashboardViewModel
    {
        public string UserFirstName { get; set; } = string.Empty;

        public string UserLastName { get; set; } = string.Empty;

        public string UserRole { get; set; } = string.Empty;

        public int NumberOfActiveUsers { get; set; }

        public int NumberOfClients { get; set; }

        public int NumberOfRequests { get; set; }

        public int NewRequests { get; set;}

        public int InProgressRequests { get; set; }

        public int FinishedRequests { get; set; }
    }
}
