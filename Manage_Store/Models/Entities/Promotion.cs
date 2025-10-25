using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("promotions")]
    public class Promotion
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string PromoCode { get; set; } = string.Empty;

        [StringLength(255)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "enum('percent','fixed')")]
        public string DiscountType { get; set; } = "percent";

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal DiscountValue { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal MinOrderAmount { get; set; } = 0;

        public int UsageLimit { get; set; } = 0;
        public int UsedCount { get; set; } = 0;

        [Column(TypeName = "enum('active','inactive')")]
        public string Status { get; set; } = "active";
    }

}
