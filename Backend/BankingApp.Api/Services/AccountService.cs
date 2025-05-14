using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Repositories;

namespace BankingApp.Api.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    
    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<AccountDTO>> GetUserAccountsAsync(int userId)
    {
        var accounts = await _accountRepository.GetByUserIdAsync(userId);
        
        return accounts.Select(a => new AccountDTO
        {
            Id = a.Id,
            AccountNumber = a.AccountNumber,
            Balance = a.Balance,
            Currency = a.Currency,
            SpendingLimit = a.SpendingLimit,
            DailyWithdrawalLimit = a.DailyWithdrawalLimit,
            Status = a.Status,
            CreatedAt = a.CreatedAt
        });
    }
    
    public async Task<AccountDTO?> GetAccountByIdAsync(int accountId, int userId)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        
        if (account == null || account.UserId != userId)
            return null;
            
        return new AccountDTO
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Currency = account.Currency,
            SpendingLimit = account.SpendingLimit,
            DailyWithdrawalLimit = account.DailyWithdrawalLimit,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }
    
    public async Task<AccountDTO> CreateAccountAsync(int userId, CreateAccountDTO createAccountDto)
    {
        // Generate unique account number
        string accountNumber;
        do
        {
            accountNumber = GenerateAccountNumber();
        } while (!await _accountRepository.IsAccountNumberUniqueAsync(accountNumber));
        
        var account = new Account
        {
            UserId = userId,
            AccountNumber = accountNumber,
            Currency = createAccountDto.Currency,
            Balance = createAccountDto.InitialBalance,
            Status = "Active"
        };
        
        await _accountRepository.AddAsync(account);
        
        return new AccountDTO
        {
            Id = account.Id,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance,
            Currency = account.Currency,
            SpendingLimit = account.SpendingLimit,
            DailyWithdrawalLimit = account.DailyWithdrawalLimit,
            Status = account.Status,
            CreatedAt = account.CreatedAt
        };
    }
    
    public async Task<bool> UpdateSpendingLimitAsync(int accountId, int userId, decimal newLimit)
    {
        var account = await _accountRepository.GetByIdAsync(accountId);
        
        if (account == null || account.UserId != userId)
            return false;
            
        account.SpendingLimit = newLimit;
        account.UpdatedAt = DateTime.UtcNow;
        
        await _accountRepository.UpdateAsync(account);
        return true;
    }
    
    private string GenerateAccountNumber()
    {
        const string chars = "0123456789";
        var random = new Random();
        
        // Format: RO + 16 digits
        return "RO" + new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}