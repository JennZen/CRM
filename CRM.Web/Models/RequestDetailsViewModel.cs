using CRM.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Web.Models
{
    public class RequestDetailsViewModel
    {
        public RequestDetailsDto? Request { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();

        public List<SelectListItem> Managers { get; set; } = new();
    }
}
