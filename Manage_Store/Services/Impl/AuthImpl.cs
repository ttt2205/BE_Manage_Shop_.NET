using Manage_Store.Data;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Security;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class AuthImpl : IAuth
    {
        private readonly AppDbContext _context;
        private readonly JwtHelper _jwt;

        public AuthImpl(AppDbContext context, JwtHelper jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        // LOGIN
        public async Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return ApiResponse<AuthResponse>.Builder()
                    .WithSuccess(false)
                    .WithStatus(401)
                    .WithMessage("Invalid username or password")
                    .Build();
            }

            var token = _jwt.GenerateToken(user);

            return ApiResponse<AuthResponse>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Login success")
                .WithData(new AuthResponse
                {
                    Token = token,
                    User = UserDto.Builder()
                        .WithId(user.Id)
                        .WithUsername(user.Username)
                        .WithFullName(user.FullName ?? "")
                        .WithRole(user.Role)
                        .Build()
                })
                .Build();
        }

        // REGISTER
        public async Task<ApiResponse<string>> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(x => x.Username == request.Username))
            {
                return ApiResponse<string>.Builder()
                    .WithSuccess(false)
                    .WithStatus(400)
                    .WithMessage("Username already exists")
                    .Build();
            }

            var user = new User
            {
                Username = request.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                FullName = request.FullName,
                Role = request.Role ?? "staff"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Builder()
                .WithSuccess(true)
                .WithStatus(201)
                .WithMessage("Register success")
                .WithData("OK")
                .Build();
        }

        // GET ACCOUNT
        public async Task<ApiResponse<object>> GetAccountAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return ApiResponse<object>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("User not found")
                    .Build();

            return ApiResponse<object>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(UserDto.Builder()
                    .WithId(user.Id)
                    .WithUsername(user.Username)
                    .WithFullName(user.FullName ?? "")
                    .WithRole(user.Role)
                    .Build())
                .Build();
        }

        // LOGOUT (JWT đơn giản → không lưu token)
        public Task<ApiResponse<string>> LogoutAsync()
        {
            return Task.FromResult(
                ApiResponse<string>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Logout success")
                .WithData("OK")
                .Build()
            );
        }
    }
}
