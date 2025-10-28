using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;


namespace Manage_Store.Services.Impl
{
    public class PromotionImpl : IPromotionService
    {
        private readonly AppDbContext _context;

        public PromotionImpl(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsCodeExistsAsync(string code)
        {
            return await _context.Promotions.AnyAsync(p => p.Status == "active" && p.PromoCode == code);
        }

        public async Task<Promotion> CreateAsync(PromotionDto promotionDto)
        {
            var promotion = new Promotion
            {
                PromoCode = promotionDto.PromoCode,
                Description = promotionDto.Description,
                DiscountType = promotionDto.DiscountType,
                DiscountValue = promotionDto.DiscountValue,
                StartDate = promotionDto.StartDate,
                EndDate = promotionDto.EndDate,
                MinOrderAmount = promotionDto.MinOrderAmount,
                UsageLimit = promotionDto.UsageLimit,
                UsedCount = promotionDto.UsedCount,
            };

            // Thêm vào DbContext
            _context.Promotions.Add(promotion);

            // Lưu thay đổi vào database
            await _context.SaveChangesAsync();

            // Trả về bản ghi vừa thêm
            return promotion;
        }

        public async Task<List<Promotion>> GetAllAsync()
        {
            return await _context.Promotions.ToListAsync();
        }

        public async Task<Promotion> GetPromotionAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return null;
            }
            return promotion;
        }

        public async Task<Promotion> UpdateAsync(int id, PromotionDto promotionDto)
        {
           var promotion = new Promotion
            {
                PromoCode = promotionDto.PromoCode,
                Description = promotionDto.Description,
                DiscountType = promotionDto.DiscountType,
                DiscountValue = promotionDto.DiscountValue,
                StartDate = promotionDto.StartDate,
                EndDate = promotionDto.EndDate,
                MinOrderAmount = promotionDto.MinOrderAmount,
                UsageLimit = promotionDto.UsageLimit,
                UsedCount = promotionDto.UsedCount,
            };
            await _context.SaveChangesAsync();

            return promotion;
        }

        public async Task DeleteAsync(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);

            // Xóa product
            _context.Promotions.Remove(promotion);

            // Lưu thay đổi vào DB
            await _context.SaveChangesAsync();
        }
    }
}
