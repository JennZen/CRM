using CRM.Application.DTOs.User;
using CRM.Web.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;

        public UserController(IUserApiClient userApiClient)
        {
            _userApiClient = userApiClient;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userApiClient.GetAllAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _userApiClient.CreateAsync(dto);

            return RedirectToAction("Index");
        }


    }
}
