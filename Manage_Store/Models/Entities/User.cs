using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Manage_Store.Models.Entities
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("full_name")]
        public string? FullName { get; set; } = string.Empty;

        [Column("role", TypeName = "enum('admin','staff', 'customer', 'manager')")]
        public string Role { get; set; } = "staff";

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation
        public ICollection<Order>? Orders { get; set; }
        public Customer? Customer { get; set; }
    }
}
