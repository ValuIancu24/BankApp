using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.Models;

public class Account
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string AccountNumber { get; set; } = string.Empty;
    
    [Required]
    public decimal Balance { get; set; } = 0;
    
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = "RON"; // Default to Romanian Leu
    
    [Required]
    public decimal SpendingLimit { get; set; } = 5000; // Default daily spending limit
    
    [Required]
    public decimal DailyWithdrawalLimit { get; set; } = 2000; // Default daily withdrawal limit
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Active"; // Active, Inactive, Blocked, etc.
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    
    // Navigation properties
    public User User { get; set; } = null!;
    public ICollection<Transaction> SourceTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> DestinationTransactions { get; set; } = new List<Transaction>();
}