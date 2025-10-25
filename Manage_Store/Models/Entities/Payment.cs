using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("payments")]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "enum('cash','card','bank_transfer','e-wallet')")]
        public string PaymentMethod { get; set; } = "cash";

        public DateTime PaymentDate { get; set; } = DateTime.Now;

        // Navigation
        public Order? Order { get; set; }
    }
}
