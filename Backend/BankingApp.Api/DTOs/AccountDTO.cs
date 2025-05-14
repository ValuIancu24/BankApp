using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.DTOs;

public class AccountDTO
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal SpendingLimit { get; set; }
    public decimal DailyWithdrawalLimit { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class CreateAccountDTO
{
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = "RON";
    
    public decimal InitialBalance { get; set; } = 0;
}

public class UpdateSpendingLimitDTO
{
    [Required]
    [Range(0, 100000)]
    public decimal NewLimit { get; set; }
}