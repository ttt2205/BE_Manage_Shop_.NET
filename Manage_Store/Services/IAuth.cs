using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IAuth
    {
        Task<User?> LoginAsync(LoginRequest request);
    }
}
