
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Models.Requests;


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

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
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
