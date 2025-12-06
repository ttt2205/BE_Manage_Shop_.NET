using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Manage_Store.Models.Entities
{
    [Table("categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Column("category_name")]
        public string CategoryName { get; set; } = string.Empty;
    }
}
