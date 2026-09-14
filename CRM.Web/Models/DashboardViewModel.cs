using CRM.Application.DTOs.Request;
using CRM.Application.DTOs.User;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public List<RequestRecentDto> RecentRequests { get; set; } = new();

        public List<UserWithRequestCountDto> ManagersWithRequestCount { get; set; } = new();

        public List<SelectListItem> Customers { get; set; } = new();

        public List<SelectListItem> Managers { get; set; } = new();
    }
}
