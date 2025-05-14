using BankingApp.Api.DTOs;
using BankingApp.Api.Models;

namespace BankingApp.Api.Services;

public interface IAccountService
{
    Task<IEnumerable<AccountDTO>> GetUserAccountsAsync(int userId);
    Task<AccountDTO?> GetAccountByIdAsync(int accountId, int userId);
    Task<AccountDTO> CreateAccountAsync(int userId, CreateAccountDTO createAccountDto);
    Task<bool> UpdateSpendingLimitAsync(int accountId, int userId, decimal newLimit);
}