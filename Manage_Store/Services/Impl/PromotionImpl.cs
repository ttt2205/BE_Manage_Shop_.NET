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
                Status = promotionReq.Status
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
            // 🔍 Tìm bản ghi khuyến mãi theo ID
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null)
            {
                throw new Exception($"Không tìm thấy khuyến mãi có ID = {id}");
            }

            // ✏️ Cập nhật các trường
            promotion.PromoCode = promotionReq.PromoCode;
            promotion.Description = promotionReq.Description;
            promotion.DiscountType = promotionReq.DiscountType;
            promotion.DiscountValue = promotionReq.DiscountValue;
            promotion.StartDate = promotionReq.StartDate;
            promotion.EndDate = promotionReq.EndDate;
            promotion.MinOrderAmount = promotionReq.MinOrderAmount;
            promotion.UsageLimit = promotionReq.UsageLimit;
            promotion.UsedCount = promotionReq.UsedCount;
            promotion.Status = promotionReq.Status;
            try
            {
                _context.Entry(promotion).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return promotion;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"❌ Lỗi cập nhật khuyến mãi: {ex.InnerException?.Message}");
                throw new Exception($"Không thể cập nhật khuyến mãi (ID={id}): {ex.InnerException?.Message}");
            }
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
