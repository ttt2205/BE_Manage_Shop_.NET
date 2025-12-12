using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(OrderReq orderReq);
        Task<List<OrderDto>> GetAllAsync();

        Task<OrderDto?> GetOrderAsync(int id);

        Task<Order> UpdateAsync(int id, OrderReq orderReq);

        Task<Order> UpdateStatus(int id, string status);

        Task<List<OrderDto>> GetOrdersByDateAsync(DateTime date);
        // Task DeleteAsync(int id);
    }
}
