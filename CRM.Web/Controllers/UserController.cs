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
            try
            {
                var users = await _userApiClient.GetAllAsync();
                return View(users);
            }
            catch (UnauthorizedAccessException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto dto)
        {
            var result = await _userApiClient.CreateAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError("", "Unable to create user");
                return View(dto);
            }
            return RedirectToAction(nameof(Index));
        }


    }
}
