using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(ProductDto ProductDto);

        Task<List<Product>> GetAllAsync();

        Task<Product> GetProductAsync(int id);

        Task<Product> UpdateAsync(int id, ProductDto productDto);

        Task DeleteAsync(int id);
    }
}
