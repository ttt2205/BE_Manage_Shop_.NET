using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(OrderReq orderReq);
        Task<List<Order>> GetAllAsync();

        Task<Order> GetOrderAsync(int id);

        Task<Order> UpdateAsync(int id, OrderReq orderReq);

        Task<Order> UpdateStatus(int id, String status);

        // Task DeleteAsync(int id);
    }
}
