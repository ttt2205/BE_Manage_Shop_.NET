using Manage_Store.Models.Dtos;

namespace Manage_Store.Responses
{
    public class AuthResponse
    {
        public string Token { get; set; } = "";
        public UserDto User { get; set; } = null!;
    }
}
