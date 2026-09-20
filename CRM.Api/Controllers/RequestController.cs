using CRM.Application.DTOs.Request;
using CRM.Application.Interfaces.Services;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequestsAsync(Status? status = null)
        {
            var requests = await _requestService.GetAllAsync(status);
            return Ok(requests);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRequestByIdAsync(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            return Ok(request);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetRequestsByUserAsync(Status? status)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var requests = await _requestService.GetByUserAsync(userId, status);
            return Ok(requests);
        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentRequestsByUserAsync()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var requests = await _requestService.GetRecentByUserAsync(userId);
            return Ok(requests);
        }


        [HttpGet("my/count")]
        public async Task<IActionResult> CountMyRequestsAsync(Status? status)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var number = await _requestService.CountByUserAndStatusAsync(userId, status);
            return Ok(number);
        }


        [HttpPost]
        public async Task<IActionResult> CreateRequestAsync([FromBody] RequestCreateDto requestCreateDto)
        {
            var createdRequest = await _requestService.AddAsync(requestCreateDto);
            return CreatedAtAction(nameof(CreateRequestAsync), new { id = createdRequest.Id }, createdRequest);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRequestAsync(int id, [FromBody] RequestUpdateDto requestUpdateDto)
        {
            if (id != requestUpdateDto.Id)
            {
                return BadRequest();
            }

            var result = await _requestService.UpdateAsync(requestUpdateDto);

            return NoContent();
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountRequestsAsync()
        {
            var count = await _requestService.GetAllAsync();
            return Ok(count.Count);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRequestAsync(int id)
        {
            var result = await _requestService.DeleteAsync(id);
            return NoContent();
        }


        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> ChangeRequestStatusAsync(int id, [FromBody] Status status)
        {

            var firstName = User.FindFirst(ClaimTypes.GivenName)?.Value ?? "";
            var lastName = User.FindFirst(ClaimTypes.Surname)?.Value ?? "";
            var authorName = $"{firstName} {lastName}".ToString();

            var result = await _requestService.ChangeStatusAsync(id, status, authorName);
            return NoContent();
        }

        [HttpPut("{id:int}/manager")]
        public async Task<IActionResult> SetRequestManagerAsync(int id, [FromBody] int managerId)
        {
            var result = await _requestService.SetManagerAsync(id, managerId);
            return NoContent();
        }
    }
}
