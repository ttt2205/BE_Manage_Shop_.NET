using System.ComponentModel.DataAnnotations;

namespace Manage_Store.Models.Dtos
{
    public class ProductDto
    {
        public int? CategoryId { get; set; }

        public int? SupplierId { get; set; }


        [Required, StringLength(100)]
        public string ProductName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Barcode { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string Unit { get; set; } = "pcs";

    }
}