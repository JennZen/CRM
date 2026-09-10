using CRM.Domain.Enums;
using CRM.Web.Interfaces;
using CRM.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var model = new DashboardViewModel()
            {
                UserFirstName = firstName,
                UserLastName = lastName,
                NewRequests = await _requestApiClient.CountRequestsAsync(Status.New),
                InProgressRequests = await _requestApiClient.CountRequestsAsync(Status.Active),
                FinishedRequests = await _requestApiClient.CountRequestsAsync(Status.Finished),
                TotalRequests = await _requestApiClient.CountRequestsAsync(null),
                NumberOfCustomers = await _customerApiClient.CountCustomersAsync(),
                NumberOfManagers = await _userApiClient.CountUsersAsync()
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
