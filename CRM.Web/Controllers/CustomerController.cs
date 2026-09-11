using CRM.Application.DTOs.Customer;
using CRM.Web.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerApiClient _customerApiClient;

        public CustomerController(ICustomerApiClient customerApiClient)
        {
            _customerApiClient = customerApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var cards = await _customerApiClient.GetAllCardsAsync();
            var numberClients = await _customerApiClient.CountCustomersAsync();

            ViewBag.NumberClients = numberClients;

            return View(cards);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerApiClient.GetByIdAsync(id);

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CustomerCreateDto dto)
        {
            await _customerApiClient.CreateAsync(dto);

            return RedirectToAction("Index");
        }
    }
}
