using CRM.Application.DTOs.Customer;
using CRM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomersAsync()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCustomerByIdAsync(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountCustomersAsync()
        {
            var count = await _customerService.CountAsync();
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomerAsync([FromBody] CustomerCreateDto customerCreateDto)
        {
            var createdCustomer = await _customerService.CreateAsync(customerCreateDto);
            return CreatedAtAction(nameof(CreateCustomerAsync), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateCustomerAsync(int id, [FromBody] CustomerUpdateDto dto)
        {
            if(dto.Id != id) 
            {
                return BadRequest("Customer ID mismatch.");
            }

            var result = await _customerService.UpdateAsync(dto);
            if (result == false)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id)
        {
            var deletedCustomer = await _customerService.DeleteAsync(id);
            if (deletedCustomer == false)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
