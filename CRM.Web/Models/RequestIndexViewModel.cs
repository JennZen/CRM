using CRM.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Web.Models
{
    public class RequestIndexViewModel
    {
        public List<RequestDetailsDto> AllRequests { get; set; } = new();

        public int NumberOfRequests { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();

        public List<SelectListItem> Managers { get; set; } = new();
    }
}
