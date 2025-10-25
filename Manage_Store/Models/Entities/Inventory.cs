using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("inventory")]
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public int Quantity { get; set; } = 0;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation
        public Product? Product { get; set; }
    }
}
