using CRM.Application.DTOs.Customer;
using CRM.Web.Interfaces;
using CRM.Web.Models;
using CRM.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerApiClient _customerApiClient;

        private readonly IRequestApiClient _requestApiClient;

        private readonly IUserApiClient _userApiClient;

        public CustomerController(ICustomerApiClient customerApiClient, IRequestApiClient requestApiClient, IUserApiClient userApiClient)
        {
            _customerApiClient = customerApiClient;
            _requestApiClient = requestApiClient;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index(string? search = null)
        {
            var cards = await _customerApiClient.GetAllCardsAsync(search);
            var numberClients = await _customerApiClient.CountCustomersAsync();

            ViewBag.NumberClients = numberClients;
            ViewBag.Search = search;

            return View(cards);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customers = await _customerApiClient.GetAllAsync();
            var managers = await _userApiClient.GetAllAsync();

            var model = new CustomerDetailsViewModel()
            {
                Customer = await _customerApiClient.GetByIdAsync(id),
                Customers = customers.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Managers = managers.Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.FirstName} {m.LastName}"
                }).ToList(),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateDto dto)
        {
            await _customerApiClient.CreateAsync(dto);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CustomerUpdateDto dto)
        {
            var success = await _customerApiClient.UpdateAsync(id, dto);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Couldn't update the customer");
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            await _customerApiClient.ArchiveAsync(id);

            return RedirectToAction("Details", new { id });
        }
    }
}
