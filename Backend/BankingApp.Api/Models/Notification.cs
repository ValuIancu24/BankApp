using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.Models;

public class Notification
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(255)]
    public string Message { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; // Info, Success, Warning, Error
    
    public bool IsRead { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign keys
    public int UserId { get; set; }
    
    public int? TransactionId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public Transaction? Transaction { get; set; }
}