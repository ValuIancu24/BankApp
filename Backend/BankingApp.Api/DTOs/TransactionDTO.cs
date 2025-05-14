using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.DTOs;

public class TransactionDTO
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsImportant { get; set; }
    public DateTime CreatedAt { get; set; }
    public int FromAccountId { get; set; }
    public int? ToAccountId { get; set; }
    public string? ToAccountNumber { get; set; }
    public string? FromAccountNumber { get; set; }
}

public class CreateTransactionDTO
{
    [Required]
    public int FromAccountId { get; set; }
    
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
    
    [MaxLength(50)]
    public string? ToAccountNumber { get; set; }
}

public class BillPaymentDTO
{
    [Required]
    public int AccountId { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string BillType { get; set; } = string.Empty;
    
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
}