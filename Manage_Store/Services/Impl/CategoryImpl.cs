using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;


namespace Manage_Store.Services.Impl
{
    public class CategoryImpl : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Category> CreateAsync(CategoryDto categoryDto)
        {
            // Map DTO sang entity
            var category = new Category
            {
                CategoryName = categoryDto.CategoryName
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

        public async Task<Category> UpdateAsync(int id, CategoryDto categoryDto)
        {

            var category = await _context.Categories.FindAsync(id);

            category.CategoryName = categoryDto.CategoryName;
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
