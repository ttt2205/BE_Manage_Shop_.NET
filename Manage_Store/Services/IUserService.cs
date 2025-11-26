using Manage_Store.Models.Dtos;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;

namespace Manage_Store.Services
{
    public interface IUserService
    {
        Task<ApiResPagination<List<UserDto>>> GetUsers(int page, int size, string? search);
        Task<ApiResponse<UserDto>> GetUserDetail(int id);
        Task<ApiResponse<UserDto>> CreateUser(CreateUserDto dto);
        Task<ApiResponse<UserDto>> UpdateUser(int id, UpdateUserDto dto);
        Task<ApiResponse<string>> DeleteUser(int id);
    }
}
