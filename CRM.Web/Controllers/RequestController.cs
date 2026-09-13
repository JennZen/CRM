using CRM.Web.Interfaces;
using CRM.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IRequestApiClient _requestApiClient;

        public RequestController(IRequestApiClient requestApiClient)
        {
            _requestApiClient = requestApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _requestApiClient.GetMyRequestsAsync();
            var numberRequests = await _requestApiClient.CountRequestsAsync(null);

            ViewBag.NumberRequests = numberRequests;

            return View(requests);
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
