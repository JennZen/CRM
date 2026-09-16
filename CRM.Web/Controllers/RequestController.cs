using CRM.Application.DTOs.Request;
using CRM.Web.Interfaces;
using CRM.Web.Models;
using CRM.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IRequestApiClient _requestApiClient;

        private readonly IUserApiClient _userApiClient;

        private readonly ICustomerApiClient _customerApiClient;

        public RequestController(IRequestApiClient requestApiClient, IUserApiClient userApiClient, ICustomerApiClient customerApiClient)
        {
            _requestApiClient = requestApiClient;
            _customerApiClient = customerApiClient;
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerApiClient.GetAllAsync();
            var managers = await _userApiClient.GetAllAsync();

            var model = new RequestIndexViewModel()
            { 
                AllRequests = await _requestApiClient.GetMyRequestsAsync(),
                NumberOfRequests = await _requestApiClient.CountRequestsAsync(null),
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

        public async Task<IActionResult> Details(int id)
        {
            var model = new RequestDetailsViewModel()
            {
                Request = await _requestApiClient.GetByIdAsync(id),
            };

            return View(model);
        }

        public async Task<IActionResult> Create(RequestCreateDto dto)
        {
            await _requestApiClient.CreateAsync(dto);
            return RedirectToAction("Index");
        }
    }
}
