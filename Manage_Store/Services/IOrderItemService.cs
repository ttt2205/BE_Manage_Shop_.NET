using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IOrderItemService
    {
       Task<OrderItem> CreateAsync(OrderItemReq orderItemReq);
    }
}
