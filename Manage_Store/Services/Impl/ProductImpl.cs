using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Models.Requests;


namespace Manage_Store.Services.Impl
{
    public class ProductImpl : IProductService
    {
        private readonly AppDbContext _context;

        public ProductImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(ProductReq productReq)
        {

            var product = new Product
            {
                ProductName = productReq.ProductName,
                Barcode = productReq.Barcode,
                Price = productReq.Price,
                Unit = productReq.Unit,
                CategoryId = productReq.CategoryId,
                SupplierId = productReq.SupplierId,
            };

            // Thêm vào DbContext
            _context.Products.Add(product);

            // Lưu thay đổi
            await _context.SaveChangesAsync();

            if (product.CategoryId.HasValue)
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            if (product.SupplierId.HasValue)
                await _context.Entry(product).Reference(p => p.Supplier).LoadAsync();

            return product;
        }

        public async Task<List<Product>> GetPaginationAsync(string search = null)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p =>
                    p.ProductName.Contains(search) ||
                    p.Category.CategoryName.Contains(search));
            }

            return await query
                .OrderBy(p => p.Id)
                .ToListAsync();
        }


        public async Task<Product> GetProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return null;
            }

            if (product.CategoryId.HasValue)
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            if (product.SupplierId.HasValue)
                await _context.Entry(product).Reference(p => p.Supplier).LoadAsync();

            return product;
        }

        public async Task<Product> UpdateAsync(int id, ProductReq productReq)
        {
            var product = await _context.Products.FindAsync(id);

            product.ProductName = productReq.ProductName;
            product.Barcode = productReq.Barcode;
            product.Price = productReq.Price;
            product.Unit = productReq.Unit;
            product.CategoryId = productReq.CategoryId;
            product.SupplierId = productReq.SupplierId;

            await _context.SaveChangesAsync();

            if (product.CategoryId.HasValue)
                await _context.Entry(product).Reference(p => p.Category).LoadAsync();

            if (product.SupplierId.HasValue)
                await _context.Entry(product).Reference(p => p.Supplier).LoadAsync();

            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            // Xóa product
            _context.Products.Remove(product);

            // Lưu thay đổi vào DB
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Select(p => new Product
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Barcode = p.Barcode,
                    Price = p.Price,
                    Unit = p.Unit,
                    CategoryId = p.CategoryId,
                    SupplierId = p.SupplierId,
                    Category = p.Category,
                    Supplier = p.Supplier
                    
                })
                .ToListAsync();
        }

    }
}
