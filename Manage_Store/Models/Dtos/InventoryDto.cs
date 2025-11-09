using Manage_Store.Models.Dtos;

namespace Manage_Store.Models.Dtos
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class InventoryImportItemDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class InventoryImportDto
    {
        public int UserId { get; set; }
        public List<InventoryImportItemDto> Items { get; set; } = new();
        public string? Note { get; set; }
    }

    public class InventoryReportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductSku { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

// Models/Requests/CreateSupplierRequest.cs
namespace Manage_Store.Models.Requests
{
    public class CreateSupplierRequest
    {
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateSupplierRequest
    {
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
    }
}

// Models/Requests/InventoryRequests.cs
namespace Manage_Store.Models.Requests
{
    public class UpdateInventoryRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class ImportInventoryRequest
    {
        public int UserId { get; set; }
        public List<InventoryImportItemDto> Items { get; set; } = new();
        public string? Note { get; set; }
    }
}