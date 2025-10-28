using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;


namespace Manage_Store.Services.Impl
{
    public class ProductImpl : IProductService
    {
        private readonly AppDbContext _context;

        public ProductImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateAsync(ProductDto productDto)
        {

            var product = new Product
            {
                ProductName = productDto.ProductName,
                Barcode = productDto.Barcode,
                Price = productDto.Price,
                Unit = productDto.Unit,
                CategoryId = productDto.CategoryId,
                SupplierId = productDto.SupplierId,
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

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
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

        public async Task<Product> UpdateAsync(int id, ProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);

            product.ProductName = productDto.ProductName;
            product.Barcode = productDto.Barcode;
            product.Price = productDto.Price;
            product.Unit = productDto.Unit;
            product.CategoryId = productDto.CategoryId;
            product.SupplierId = productDto.SupplierId;

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
    }
}
