using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("inventory_audit_items")]
    public class InventoryAuditItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int SessionId { get; set; }

        [Required]
        public int SystemQuantity { get; set; }

        [Required]
        public int ActualQuantity { get; set; }

        [Required]
        public int Difference { get; set; }

        public string Note { get; set; } = string.Empty;

        // Navigation
        public Product? Product { get; set; }
        public AuditSessions? AuditSession { get; set; }
    }
}

        