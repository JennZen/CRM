namespace CRM.Web.Models
{
    public class DashboardViewModel
    {
        public string UserFirstName { get; set; } = string.Empty;

        public string UserLastName { get; set; } = string.Empty;

        public int NumberOfCustomers { get; set; }

        public int NumberOfManagers { get; set; }

        public int TotalRequests { get; set; }

        public int NewRequests { get; set;}

        public int InProgressRequests { get; set; }

        public int FinishedRequests { get; set; }
    }
}
