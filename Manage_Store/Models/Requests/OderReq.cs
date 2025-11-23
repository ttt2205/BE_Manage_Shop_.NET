using System.ComponentModel.DataAnnotations;

namespace Manage_Store.Models.Requests
{
    public class OrderReq
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public int? PromotionId { get; set; }

        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }

        // Danh sách chi tiết đơn hàng
        public List<OrderItemReq>? Items { get; set; }
    }
}
