using Manage_Store.Models.Requests;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Manage_Store.Responses;


namespace Manage_Store.Services
{
    public interface ICategoryService
    {
        Task<Category> CreateAsync(CategoryReq categoryReq);

        Task<ApiResPagination<List<Category>>> GetPaginationAsync(int page, int pageSize);

        Task<Category> GetCategoryAsync(int id);

        Task<Category> UpdateAsync(int id, CategoryReq categoryReq);
   
        Task DeleteAsync(int id);

        Task<List<Category>> GetCategoriesAsync();
    }
}
