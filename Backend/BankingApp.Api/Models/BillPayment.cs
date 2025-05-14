using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.Models;

public class BillPayment
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string BillType { get; set; } = string.Empty; // Electricity, Water, Gas, etc.
    
    [Required]
    [MaxLength(50)]
    public string Provider { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string BillNumber { get; set; } = string.Empty;
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = "RON";
    
    [MaxLength(255)]
    public string? Note { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Completed";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign keys
    public int TransactionId { get; set; }
    
    public int AccountId { get; set; }
    
    // Navigation properties
    public Transaction Transaction { get; set; } = null!;
    public Account Account { get; set; } = null!;
}