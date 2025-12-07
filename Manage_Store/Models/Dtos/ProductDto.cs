namespace Manage_Store.Models.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string? Barcode { get; set; }

        public decimal Price { get; set; }
        public string Unit { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
