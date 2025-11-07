
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;

namespace Manage_Store.Services.Impl
{
    public class InventoryService : IInventory
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Inventory>> GetAllInventoryAsync()
        {
            return await _context.Set<Inventory>()
                                 .Include(i => i.Product)
                                 .ToListAsync();
        }

        
    }
}