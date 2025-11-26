using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Security;
using Manage_Store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _auth;
        private readonly JwtHelper _jwtHelper;

        public AuthController(IAuth auth, JwtHelper jwtHelper)
        {
            _auth = auth;
            _jwtHelper = jwtHelper;
        }

        // LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var res = await _auth.LoginAsync(request);
            return StatusCode(res.Status, res);
        }

        // REGISTER
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var res = await _auth.RegisterAsync(request);
            return StatusCode(res.Status, res);
        }

        // GET ACCOUNT
        [Authorize]
        [HttpGet("account")]
        public async Task<IActionResult> GetAccount()
        {
            var token = Request.Headers["Authorization"]
                .ToString().Replace("Bearer ", "");

            var userId = _jwtHelper.GetUserId(token);

            if (userId == null)
                return Unauthorized("Invalid token");

            var res = await _auth.GetAccountAsync((int)userId);
            return StatusCode(res.Status, res);
        }

        // LOGOUT
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var res = await _auth.LogoutAsync();
            return StatusCode(res.Status, res);
        }
    }
}
