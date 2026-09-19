using CRM.Domain.Enums;
using CRM.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CRM.Web.ViewComponents
{
    public class SidebarCountsViewComponent : ViewComponent
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public SidebarCountsViewComponent(IHttpClientFactory httpClientFactory, IHttpContextAccessor accessor)
        {
            _httpClient = httpClientFactory.CreateClient("CRM.Api"); ;
            _httpContextAccessor = accessor;
        }

        private void AttachToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            AttachToken();

            var role = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var currentController = ViewContext.RouteData.Values["controller"]?.ToString() ?? "";
            var customersCount = await _httpClient.GetFromJsonAsync<int>("api/customer/count");

            int requestsCount = 0, userCount = 0;

            if(role == UserRole.Admin.ToString())
            {
                requestsCount = await _httpClient.GetFromJsonAsync<int>("api/request/count");
                userCount = await _httpClient.GetFromJsonAsync<int>("api/user/count");
            }
            else requestsCount = await _httpClient.GetFromJsonAsync<int>("api/request/my/count");

            return View(new SidebarCountsViewModel() 
            { 
                CustomersCount = customersCount, 
                RequestsCount = requestsCount,
                UserCount = userCount,
                CurrentController = currentController,
            });
        }
    }
}
