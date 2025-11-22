using System.ComponentModel.DataAnnotations;

public class SubmitAuditItemRequest
{
    [Required]
    public int SessionId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    [Range(0, int.MaxValue)]
    public int ActualQuantity { get; set; }
    public string Note { get; set; } = string.Empty;
}