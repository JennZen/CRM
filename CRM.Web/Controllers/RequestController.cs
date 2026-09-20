using CRM.Application.DTOs.Request;
using CRM.Domain.Enums;
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

        public async Task<IActionResult> Index(Status? status)
        {
            var customers = await _customerApiClient.GetAllActiveAsync();
            var managers = await _userApiClient.GetAllActiveAsync();
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var model = new RequestIndexViewModel()
            {
                AllRequests = (role == UserRole.Admin.ToString())
                              ? await _requestApiClient.GetAllAsync(status)
                              : await _requestApiClient.GetMyRequestsAsync(status),

                NumberOfRequests = (role == UserRole.Admin.ToString())
                              ? await _requestApiClient.CountRequestsAsync()
                              : await _requestApiClient.CountMyRequestsAsync(null),

                CurrentStatus = status,
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
            var customers = await _customerApiClient.GetAllActiveAsync();
            var managers = await _userApiClient.GetAllActiveAsync();

            var model = new RequestDetailsViewModel()
            {
                Request = await _requestApiClient.GetByIdAsync(id),
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
        public async Task<IActionResult> Create(RequestCreateDto dto)
        {
            await _requestApiClient.CreateAsync(dto);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, RequestUpdateDto dto)
        {
            dto.Id = id;
            var success = await _requestApiClient.UpdateAsync(id, dto);

            if (!success)
            {
                TempData["Error"] = "Failed to update request.";
            }

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id, Status status)
        {
            await _requestApiClient.ChangeRequestStatusAsync(id, status);

            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> SetManager(int id, int managerId)
        {
            await _requestApiClient.SetRequestManagerAsync(id, managerId);

            return RedirectToAction("Details", new { id });
        }
    }
}
