using Manage_Store.Models.Entities;

namespace Manage_Store.Services
{
    public interface IInventory
    {
        Task<IEnumerable<Inventory>> GetAllInventoryAsync();
    }
}