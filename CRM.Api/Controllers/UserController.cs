using CRM.Application.DTOs.User;
using CRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("with-request-count")]
        public async Task<IActionResult> GetUsersWithNumberOfRequestsAsync()
        {
            var usersWithRequests = await _userService.GetUsersWithNumberOfRequestsAsync();
            return Ok(usersWithRequests);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetByIdAsync(id);

            if(user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountUsersAsync()
        {
            return Ok(await _userService.CountAsync());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result == false) return NotFound();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> DeactivateUserAsync(int id)
        {
            await _userService.DeactivateAsync(id);

            return NoContent();
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] UserUpdateDto dto)
        {
            if(dto.Id != id)
            {
                return BadRequest("User Id mismatch.");
            }

            var result = await _userService.UpdateAsync(dto);
            if (result == false) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto dto)
        {
            var createdUser = await _userService.AddAsync(dto);
            return CreatedAtAction(nameof(CreateUserAsync), new { id = createdUser.Id }, createdUser);
        }
    }
}
