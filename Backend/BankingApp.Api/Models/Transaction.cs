using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.Models;

public class Transaction
{
    public int Id { get; set; }
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; // Transfer, Deposit, Withdrawal, BillPayment
    
    [MaxLength(255)]
    public string? Note { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Cancelled
    
    public bool IsImportant { get; set; } = false;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign keys
    public int FromAccountId { get; set; }
    
    public int? ToAccountId { get; set; }
    
    [MaxLength(50)]
    public string? ToAccountNumber { get; set; }
    
    [MaxLength(50)]
    public string? FromAccountNumber { get; set; }
    
    // Navigation properties
    public Account FromAccount { get; set; } = null!;
    public Account? ToAccount { get; set; }
}