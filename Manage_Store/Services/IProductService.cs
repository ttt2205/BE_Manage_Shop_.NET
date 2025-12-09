using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(ProductReq ProductReq);

        Task<List<Product>> GetPaginationAsync(string search = null);

        Task<List<Product>> GetAllAsync();

        Task<Product> GetProductAsync(int id);

        Task<Product> UpdateAsync(int id, ProductReq productReq);

        Task DeleteAsync(int id);
    }
}
