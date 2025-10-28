using System.ComponentModel.DataAnnotations;
namespace Manage_Store.Models.Dtos
{
    public class CategoryDto
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; } = string.Empty;
    }
}