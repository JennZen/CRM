using CRM.Application.DTOs.Customer;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Web.Models
{
    public class CustomerDetailsViewModel
    {
        public CustomerDetailsDto? Customer { get; set; }

        public List<SelectListItem> Customers { get; set; } = new();

        public List<SelectListItem> Managers { get; set; } = new();
    }
}
