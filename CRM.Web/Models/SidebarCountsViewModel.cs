namespace CRM.Web.Models
{
    public class SidebarCountsViewModel
    {
        public int CustomersCount { get; set; }
        
        public int RequestsCount { get; set; }

        public int UserCount { get; set; }

        public string CurrentController { get; set; } = "";
    }
}
