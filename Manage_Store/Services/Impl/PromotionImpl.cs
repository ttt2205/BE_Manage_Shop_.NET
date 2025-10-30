using Manage_Store.Models.Dtos;
using Manage_Store.Models.Entities;
using Manage_Store.Data;
using Microsoft.EntityFrameworkCore;
using Manage_Store.Models.Requests;


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

        public async Task<Promotion> CreateAsync(PromotionReq promotionReq)
        {
            var promotion = new Promotion
            {
                PromoCode = promotionReq.PromoCode,
                Description = promotionReq.Description,
                DiscountType = promotionReq.DiscountType,
                DiscountValue = promotionReq.DiscountValue,
                StartDate = promotionReq.StartDate,
                EndDate = promotionReq.EndDate,
                MinOrderAmount = promotionReq.MinOrderAmount,
                UsageLimit = promotionReq.UsageLimit,
                UsedCount = promotionReq.UsedCount,
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

        public async Task<Promotion> UpdateAsync(int id, PromotionReq promotionReq)
        {
           var promotion = new Promotion
            {
                PromoCode = promotionReq.PromoCode,
                Description = promotionReq.Description,
                DiscountType = promotionReq.DiscountType,
                DiscountValue = promotionReq.DiscountValue,
                StartDate = promotionReq.StartDate,
                EndDate = promotionReq.EndDate,
                MinOrderAmount = promotionReq.MinOrderAmount,
                UsageLimit = promotionReq.UsageLimit,
                UsedCount = promotionReq.UsedCount,
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
