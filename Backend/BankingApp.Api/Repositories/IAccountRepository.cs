using BankingApp.Api.Models;

namespace BankingApp.Api.Repositories;

public interface IAccountRepository : IRepository<Account>
{
    Task<IEnumerable<Account>> GetByUserIdAsync(int userId);
    Task<Account?> GetByAccountNumberAsync(string accountNumber);
    Task<bool> UpdateBalanceAsync(int accountId, decimal amount);
    Task<bool> IsAccountNumberUniqueAsync(string accountNumber);
}