using CRM.Web.Interfaces;
using CRM.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Claims;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CRM.Web.Controllers
{
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

        public IActionResult Index()
        {
            var firstName = User.FindFirst(System.Security.Claims.ClaimTypes.GivenName)?.Value;
            var lastName = User.FindFirst(System.Security.Claims.ClaimTypes.Surname)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var model = new DashboardViewModel()
            {
                UserFirstName = firstName,
                UserLastName = lastName,
                UserRole = role,
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
