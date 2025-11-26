using Manage_Store.Models.Requests;
using Manage_Store.Services;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /api/v1/user?page=1&size=10&search=a
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int size = 10, [FromQuery] string? search = "")
        {
            var res = await _userService.GetUsers(page, size, search);
            return StatusCode(res.Status, res);
        }

        // GET: /api/v1/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(int id)
        {
            var res = await _userService.GetUserDetail(id);
            return StatusCode(res.Status, res);
        }

        // POST: /api/v1/user
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var res = await _userService.CreateUser(dto);
            return StatusCode(res.Status, res);
        }

        // PUT: /api/v1/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var res = await _userService.UpdateUser(id, dto);
            return StatusCode(res.Status, res);
        }

        // DELETE: /api/v1/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _userService.DeleteUser(id);
            return StatusCode(res.Status, res);
        }
    }
}
