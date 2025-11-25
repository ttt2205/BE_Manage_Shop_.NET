using System.ComponentModel.DataAnnotations;

public class FinalizeAuditRequest
{
    [Required]
    public int SessionId { get; set; }
    public string FinalNote { get; set; } = string.Empty;
}