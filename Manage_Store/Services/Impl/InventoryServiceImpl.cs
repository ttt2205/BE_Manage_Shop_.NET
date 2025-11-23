//using Manage_Store.Data;
//using Manage_Store.Models.Dtos;
//using Manage_Store.Models.Entities;
//using Manage_Store.Models.Requests;
//using Manage_Store.Responses;
//using Microsoft.EntityFrameworkCore;

//namespace Manage_Store.Services.Impl
//{
//    public class InventoryServiceImpl : IInventoryService
//    {
//        private readonly AppDbContext _context;

//        public InventoryServiceImpl(AppDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<ApiResPagination<List<InventoryDto>>> GetAllAsync(int page = 1, int pageSize = 10)
//        {
//            try
//            {
//                var totalItems = await _context.Inventory.CountAsync();
//                var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

//                var inventories = await _context.Inventory
//                    .Include(i => i.Product)
//                    .OrderBy(i => i.Id)
//                    .Skip((page - 1) * pageSize)
//                    .Take(pageSize)
//                    .Select(i => new InventoryDto
//                    {
//                        Id = i.Id,
//                        ProductId = i.ProductId,
//                        ProductName = i.Product != null ? i.Product.ProductName : "",
//                        Quantity = i.Quantity,
//                        UpdatedAt = i.UpdatedAt
//                    })
//                    .ToListAsync();

//                return ApiResPagination<List<InventoryDto>>.Builder()
//                    .WithSuccess(true)
//                    .WithStatus(200)
//                    .WithMessage("Get inventory successfully")
//                    .WithResult(inventories)
//                    .WithMeta(new Meta
//                    {
//                        CurrentPage = page,
//                        PageSize = pageSize,
//                        TotalPage = totalPages,
//                        TotalItems = totalItems
//                    })
//                    .Build();
//            }
//            catch (Exception ex)
//            {
//                return ApiResPagination<List<InventoryDto>>.Builder()
//                    .WithSuccess(false)
//                    .WithStatus(500)
//                    .WithMessage($"Error: {ex.Message}")
//                    .Build();
//            }
//        }

//        public async Task<ApiResponse<InventoryDto>> GetByProductIdAsync(int productId)
//        {
//            try
//            {
//                var inventory = await _context.Inventory
//                    .Include(i => i.Product)
//                    .FirstOrDefaultAsync(i => i.ProductId == productId);

//                if (inventory == null)
//                {
//                    return ApiResponse<InventoryDto>.Builder()
//                        .WithSuccess(false)
//                        .WithStatus(404)
//                        .WithMessage("Inventory not found")
//                        .Build();
//                }

//                var dto = new InventoryDto
//                {
//                    Id = inventory.Id,
//                    ProductId = inventory.ProductId,
//                    ProductName = inventory.Product?.ProductName ?? "",
//                    Quantity = inventory.Quantity,
//                    UpdatedAt = inventory.UpdatedAt
//                };

//                return ApiResponse<InventoryDto>.Builder()
//                    .WithSuccess(true)
//                    .WithStatus(200)
//                    .WithMessage("Get inventory successfully")
//                    .WithData(dto)
//                    .Build();
//            }
//            catch (Exception ex)
//            {
//                return ApiResponse<InventoryDto>.Builder()
//                    .WithSuccess(false)
//                    .WithStatus(500)
//                    .WithMessage($"Error: {ex.Message}")
//                    .Build();
//            }
//        }

//        public async Task<ApiResponse<bool>> UpdateInventoryAsync(UpdateInventoryRequest request)
//        {
//            try
//            {
//                var inventory = await _context.Inventory
//                    .FirstOrDefaultAsync(i => i.ProductId == request.ProductId);

//                if (inventory == null)
//                {
//                    // Create new inventory record if not exists
//                    inventory = new Inventory
//                    {
//                        ProductId = request.ProductId,
//                        Quantity = request.Quantity,
//                        UpdatedAt = DateTime.Now
//                    };
//                    _context.Inventory.Add(inventory);
//                }
//                else
//                {
//                    inventory.Quantity = request.Quantity;
//                    inventory.UpdatedAt = DateTime.Now;
//                }

//                await _context.SaveChangesAsync();

//                return ApiResponse<bool>.Builder()
//                    .WithSuccess(true)
//                    .WithStatus(200)
//                    .WithMessage("Update inventory successfully")
//                    .WithData(true)
//                    .Build();
//            }
//            catch (Exception ex)
//            {
//                return ApiResponse<bool>.Builder()
//                    .WithSuccess(false)
//                    .WithStatus(500)
//                    .WithMessage($"Error: {ex.Message}")
//                    .Build();
//            }
//        }

//        public async Task<ApiResponse<bool>> ImportInventoryAsync(ImportInventoryRequest request)
//        {
//            using var transaction = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                foreach (var item in request.Items)
//                {
//                    var inventory = await _context.Inventory
//                        .FirstOrDefaultAsync(i => i.ProductId == item.ProductId);

//                    if (inventory == null)
//                    {
//                        inventory = new Inventory
//                        {
//                            ProductId = item.ProductId,
//                            Quantity = item.Quantity,
//                            UpdatedAt = DateTime.Now
//                        };
//                        _context.Inventory.Add(inventory);
//                    }
//                    else
//                    {
//                        inventory.Quantity += item.Quantity;
//                        inventory.UpdatedAt = DateTime.Now;
//                    }
//                }

//                await _context.SaveChangesAsync();
//                await transaction.CommitAsync();

//                return ApiResponse<bool>.Builder()
//                    .WithSuccess(true)
//                    .WithStatus(200)
//                    .WithMessage("Import inventory successfully")
//                    .WithData(true)
//                    .Build();
//            }
//            catch (Exception ex)
//            {
//                await transaction.RollbackAsync();
//                return ApiResponse<bool>.Builder()
//                    .WithSuccess(false)
//                    .WithStatus(500)
//                    .WithMessage($"Error: {ex.Message}")
//                    .Build();
//            }
//        }

//        public async Task<ApiResponse<List<InventoryReportDto>>> GetInventoryReportAsync(DateTime? startDate, DateTime? endDate)
//        {
//            try
//            {
//                var query = _context.Inventory
//                    .Include(i => i.Product)
//                    .ThenInclude(p => p!.Category)
//                    .AsQueryable();

//                if (startDate.HasValue)
//                {
//                    query = query.Where(i => i.UpdatedAt >= startDate.Value);
//                }

//                if (endDate.HasValue)
//                {
//                    query = query.Where(i => i.UpdatedAt <= endDate.Value);
//                }

//                var report = await query
//                    .Select(i => new InventoryReportDto
//                    {
//                        ProductId = i.ProductId,
//                        ProductName = i.Product != null ? i.Product.ProductName : "",
//                        ProductSku = i.Product != null && i.Product.Barcode != null ? i.Product.Barcode : "",
//                        CategoryName = i.Product != null && i.Product.Category != null ? i.Product.Category.CategoryName : "",
//                        Quantity = i.Quantity,
//                        Price = i.Product != null ? i.Product.Price : 0,
//                        UpdatedAt = i.UpdatedAt
//                    })
//                    .OrderBy(r => r.ProductName)
//                    .ToListAsync();

//                return ApiResponse<List<InventoryReportDto>>.Builder()
//                    .WithSuccess(true)
//                    .WithStatus(200)
//                    .WithMessage("Get inventory report successfully")
//                    .WithData(report)
//                    .Build();
//            }
//            catch (Exception ex)
//            {
//                return ApiResponse<List<InventoryReportDto>>.Builder()
//                    .WithSuccess(false)
//                    .WithStatus(500)
//                    .WithMessage($"Error: {ex.Message}")
//                    .Build();
//            }
//        }
//    }
//}