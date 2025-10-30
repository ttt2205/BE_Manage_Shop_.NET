using System.ComponentModel.DataAnnotations;


namespace Manage_Store.Models.Requests
{
    public class PromotionReq
    {
        [Required, StringLength(50)]
        public string PromoCode { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        public string DiscountType { get; set; } = "percent";

        [Required]
        public decimal DiscountValue { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public decimal MinOrderAmount { get; set; } = 0;

        public int UsageLimit { get; set; } = 0;
        public int UsedCount { get; set; } = 0;

    }
}