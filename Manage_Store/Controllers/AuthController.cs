using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    // localhost:****/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;

        public AuthController(IAuth auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var res = await _auth.LoginAsync(request);

            var errorResponse = ApiResponse<object>.Builder()
                .WithSuccess(false)
                .WithStatus(401)
                .WithMessage("Invalid username or password")
                .Build();

            return Unauthorized(errorResponse);
        }
    }
}
