
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Models.Requests;
using Manage_Store.Models.Dtos;


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

        public async Task<ResPagination<List<Category>>> GetAllAsync(int page, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            var totalItems = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync(); // đây là List<Category>
            Console.WriteLine(data.Count);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            return new ResPagination<List<Category>>
            {
                result = data, // <- phải có dữ liệu
                meta = new ResPagination<List<Category>>.Meta
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalItems = totalItems,
                    totalPage = totalPages
                }
            };
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

    }
}
