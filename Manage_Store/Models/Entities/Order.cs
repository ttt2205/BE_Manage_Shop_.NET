using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("customer_id")]
        public int? CustomerId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }

        // [Column("promo_id")]
        // public int? PromoId { get; set; }

        [Column("promotion_id")]
        public int? PromotionId { get; set; }

        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column("status", TypeName = "enum('pending','paid','canceled')")]
        public string Status { get; set; } = "pending";

        [Column("total_amount", TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

        [Column("discount_amount", TypeName = "decimal(10,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        // Navigation
        public Customer? Customer { get; set; }
        public User? User { get; set; }
        public Promotion? Promotion { get; set; }

      public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
