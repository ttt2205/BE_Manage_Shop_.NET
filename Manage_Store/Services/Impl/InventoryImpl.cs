using Manage_Store.Data;
using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;
using Manage_Store.Responses;
using Manage_Store.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage_Store.Services.Impl
{
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        // 1. GetAllAsync (Dùng ApiResPagination -> Gi? nguyên .WithResult)
        public async Task<ApiResPagination<List<InventoryDto>>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            var query = _context.Inventory
                .Include(i => i.Product)
                .AsNoTracking();

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var items = await query
                .OrderByDescending(x => x.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new InventoryDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    ProductName = x.Product != null ? x.Product.ProductName : "Unknown Product",
                    Quantity = x.Quantity,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();

            var meta = new Meta
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPage = totalPages
            };

            return ApiResPagination<List<InventoryDto>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("L?y danh sách thành công")
                .WithResult(items) // ApiResPagination dùng WithResult
                .WithMeta(meta)
                .Build();
        }

        // 2. GetByProductIdAsync (Dùng ApiResponse -> S?a thành .WithData)
        public async Task<ApiResponse<InventoryDto>> GetByProductIdAsync(int productId)
        {
            var item = await _context.Inventory
                .Include(i => i.Product)
                .FirstOrDefaultAsync(x => x.ProductId == productId);

            if (item == null)
            {
                return ApiResponse<InventoryDto>.Builder()
                    .WithSuccess(false)
                    .WithStatus(404)
                    .WithMessage("Không tìm th?y s?n ph?m")
                    .Build();
            }

            var dto = new InventoryDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product?.ProductName ?? "N/A",
                Quantity = item.Quantity,
                UpdatedAt = item.UpdatedAt
            };

            return ApiResponse<InventoryDto>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(dto) // <--- ?Ã S?A: Thay WithResult b?ng WithData
                .Build();
        }

        // 3. UpdateInventoryAsync (Dùng ApiResponse -> S?a thành .WithData)
        public async Task<ApiResponse<bool>> UpdateInventoryAsync(UpdateInventoryRequest request)
        {
            // Logic c?p nh?t (Ví d? m?u)
            var inventory = await _context.Inventory.FirstOrDefaultAsync(x => x.ProductId == request.ProductId);
            if (inventory == null)
            {
                // N?u ch?a có thì t?o m?i (ho?c tr? l?i tùy nghi?p v?)
                inventory = new Inventory
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    UpdatedAt = DateTime.Now
                };
                _context.Inventory.Add(inventory);
            }
            else
            {
                inventory.Quantity = request.Quantity;
                inventory.UpdatedAt = DateTime.Now;
                _context.Inventory.Update(inventory);
            }

            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("C?p nh?t thành công")
                .WithData(true) // <--- ?Ã S?A: Thay WithResult b?ng WithData
                .Build();
        }

        // 4. ImportInventoryAsync (Dùng ApiResponse -> S?a thành .WithData)
        public async Task<ApiResponse<bool>> ImportInventoryAsync(ImportInventoryRequest request)
        {
            // Logic Import m?u...
            // foreach(var item in request.Items) { ... }
            // await _context.SaveChangesAsync();

            return ApiResponse<bool>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(true) // <--- ?Ã S?A: Thay WithResult b?ng WithData
                .Build();
        }

        // 5. GetInventoryReportAsync (Dùng ApiResponse -> S?a thành .WithData)
        public async Task<ApiResponse<List<InventoryReportDto>>> GetInventoryReportAsync(DateTime? startDate, DateTime? endDate)
        {
            var emptyList = new List<InventoryReportDto>();

            return ApiResponse<List<InventoryReportDto>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithData(emptyList) // <--- ?Ã S?A: Thay WithResult b?ng WithData
                .Build();
        }
    }
}