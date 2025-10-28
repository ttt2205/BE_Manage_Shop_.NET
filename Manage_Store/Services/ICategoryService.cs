using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Manage_Store.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateAsync(CategoryDto categoryDto);
        Task<List<Category>> GetAllAsync();
        Task<Category> GetCategoryAsync(int id);

        Task<Category> UpdateAsync(int id, CategoryDto categoryDto);

        Task DeleteAsync(int id);
    }
}
