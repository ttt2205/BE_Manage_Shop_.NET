
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Models.Requests;
using Manage_Store.Models.Dtos;
using Manage_Store.Responses;


namespace Manage_Store.Services.Impl
{
    public class CategoryImpl : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(CategoryReq categoryReq)
        {
            // Map DTO sang entity
            var category = new Category
            {
                CategoryName = categoryReq.CategoryName
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ApiResPagination<List<Category>>> GetPaginationAsync(int page, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            // Tổng số item
            int totalItems = await query.CountAsync();

            // Lấy dữ liệu phân trang
            var items = await query
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Tạo Meta
            var meta = new Meta
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPage = totalPages
            };

            // Trả về ApiResponse với builder
            var response = ApiResPagination<List<Category>>.Builder()
                .WithSuccess(true)
                .WithStatus(200)
                .WithMessage("Danh sách category")
                .WithResult(items)
                .WithMeta(meta)
                .Build();

            return response;
        }



        public async Task<Category> GetCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return null;
            }
            return category;
        }

        public async Task<Category> UpdateAsync(int id, CategoryReq categoryReq)
        {

            var category = await _context.Categories.FindAsync(id);

            category.CategoryName = categoryReq.CategoryName;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }
    }
}
