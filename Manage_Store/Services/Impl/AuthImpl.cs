using Manage_Store.Data;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class AuthImpl: IAuth
    {
        private readonly AppDbContext _context;
        public AuthImpl(AppDbContext context) {
            _context = context;
        }

        public async Task<User?> LoginAsync(LoginRequest request)
        {
            try
            {
                // Tìm user theo username
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == request.Username);

                if (user == null) {      
                    throw new Exception("User not found");
                }

                // Kiểm tra mật khẩu (ở đây là so sánh trực tiếp, thực tế nên hash)
                if (user.Password != request.Password)
                {
                    throw new Exception("Invalid password");
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during login: {ex.Message}");
                return null;
            }
        }

    }
}
