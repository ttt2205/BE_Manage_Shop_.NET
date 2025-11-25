using System.ComponentModel.DataAnnotations;
namespace Manage_Store.Models.Requests
{
    public class CategoryReq
    {
        [Required(ErrorMessage = "Category name is required")]
        public string CategoryName { get; set; } = string.Empty;
    }
}