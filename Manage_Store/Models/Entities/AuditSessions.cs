using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("audit_sessions")]
    public class AuditSessions
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public string Note { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "enum('in_progress','completed','cancelled')")]
        public string Status { get; set; } = string.Empty;
        // Navigation
        public User? User { get; set; }
        public ICollection<InventoryAuditItem> AuditItems { get; set; } = new List<InventoryAuditItem>();
    }
}