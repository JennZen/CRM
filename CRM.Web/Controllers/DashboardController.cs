using CRM.Domain.Enums;
using CRM.Web.Interfaces;
using CRM.Web.Models;
using DevExpress.Data.Mask.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        private readonly IRequestApiClient _requestApiClient;

        private readonly ICustomerApiClient _customerApiClient;

        public DashboardController(IUserApiClient userApiClient, IRequestApiClient requestApiClient, ICustomerApiClient customerApiClient)
        {
            _userApiClient = userApiClient;
            _requestApiClient = requestApiClient;
            _customerApiClient = customerApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var firstName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
            var lastName = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            var customers = await _customerApiClient.GetAllActiveAsync();
            var managers = await _userApiClient.GetAllActiveAsync();

            var model = new DashboardViewModel()
            {
                UserFirstName = firstName,
                UserLastName = lastName,
                NewRequests = await _requestApiClient.CountMyRequestsAsync(Status.New),
                InProgressRequests = await _requestApiClient.CountMyRequestsAsync(Status.Active),
                FinishedRequests = await _requestApiClient.CountMyRequestsAsync(Status.Finished),
                TotalRequests = await _requestApiClient.CountMyRequestsAsync(null),
                NumberOfCustomers = await _customerApiClient.CountCustomersAsync(),
                NumberOfManagers = await _userApiClient.CountUsersAsync(),
                RecentRequests = await _requestApiClient.GetRecentRequestsByUserAsync(),
                ManagersWithRequestCount = await _userApiClient.GetWithNumberOfRequestsAsync(),
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


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
