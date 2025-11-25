using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
   [Table("products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }

        [Column("supplier_id")]
        public int? SupplierId { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; } = string.Empty;

        [Column("barcode")]
        public string? Barcode { get; set; }

        [Column("price", TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Column("unit")]
        public string Unit { get; set; } = "pcs";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Navigation
        public Category? Category { get; set; }
        public Supplier? Supplier { get; set; }
        public ICollection<OrderItem>? OrderItems { get; set; }
    }
}
