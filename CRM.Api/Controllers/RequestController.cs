using CRM.Application.DTOs.Request;
using CRM.Application.Interfaces.Services;
using CRM.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRequestsAsync()
        {
            var requests = await _requestService.GetAllAsync();
            return Ok(requests);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetRequestByIdAsync(int id)
        {
            var request = await _requestService.GetByIdAsync(id);
            if (request == null)
            {
                return NotFound();
            }
            return Ok(request);
        }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetRequestsByUserAsync(int userId)
        {
            var requests = await _requestService.GetByUserAsync(userId);
            return Ok(requests);
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
            if (!result)
            {
                return NotFound();
            }

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
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }


        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> ChangeRequestStatusAsync(int id, [FromBody] string status)
        {
            if (!Enum.TryParse<Status>(status, true, out var parsedStatus))
            {
                return BadRequest("Invalid status value.");
            }
            var result = await _requestService.ChangeStatusAsync(id, parsedStatus);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id:int}/manager")]
        public async Task<IActionResult> SetRequestManagerAsync(int id, [FromBody] int managerId)
        {
            var result = await _requestService.SetManagerAsync(id, managerId);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
