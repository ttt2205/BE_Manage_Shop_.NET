using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;

namespace Manage_Store.Services
{
    public interface IAuth
    {
        Task<ApiResponse<AuthResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<object>> GetAccountAsync(int userId);
        Task<ApiResponse<string>> LogoutAsync();
    }
}
