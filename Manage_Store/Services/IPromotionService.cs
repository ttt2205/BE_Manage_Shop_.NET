using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Models.Requests;

namespace Manage_Store.Services
{
    public interface IPromotionService
    {
        Task<Promotion> CreateAsync(PromotionDto promotionDto);
        Task<bool> IsCodeExistsAsync(string code);


        Task<List<Promotion>> GetAllAsync();

        Task<Promotion> GetPromotionAsync(int id);

        Task<Promotion> UpdateAsync(int id, PromotionDto promotionDto);

        Task DeleteAsync(int id);
    }
}
