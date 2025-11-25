using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(ProductReq ProductReq);

        Task<List<Product>> GetAllAsync(string search = null);

        Task<Product> GetProductAsync(int id);

        Task<Product> UpdateAsync(int id, ProductReq productReq);

        Task DeleteAsync(int id);
    }
}
