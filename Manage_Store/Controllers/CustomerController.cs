using Manage_Store.Models.Dtos;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/v1/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // Pagination
        [HttpGet]
        public async Task<IActionResult> GetPagination([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? search = "")
        {
            var res = await _service.GetPagination(page, size, search);
            return StatusCode(res.Status, res);
        }

        // Get all
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var res = await _service.GetAll();
            return Ok(res);
        }

        // Get by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _service.GetById(id);
            return StatusCode(res.Status, res);
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var res = await _service.CreateCustomer(dto);
            return StatusCode(res.Status, res);
        }

        // Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
        {
            var res = await _service.UpdateCustomer(id, dto);
            return StatusCode(res.Status, res);
        }

        // Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _service.DeleteCustomer(id);
            return StatusCode(res.Status, res);
        }
    }
}
