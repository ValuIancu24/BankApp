using BankingApp.Api.Data;
using BankingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _context;
    
    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Account>> GetAllAsync()
    {
        return await _context.Accounts.ToListAsync();
    }
    
    public async Task<Account?> GetByIdAsync(int id)
    {
        return await _context.Accounts.FindAsync(id);
    }
    
    public async Task<Account> AddAsync(Account entity)
    {
        await _context.Accounts.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }
    
    public async Task UpdateAsync(Account entity)
    {
        _context.Accounts.Update(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Account entity)
    {
        _context.Accounts.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public async Task<IEnumerable<Account>> GetByUserIdAsync(int userId)
    {
        return await _context.Accounts
            .Where(a => a.UserId == userId)
            .ToListAsync();
    }
    
    public async Task<Account?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _context.Accounts
            .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
    }
    
    public async Task<bool> UpdateBalanceAsync(int accountId, decimal amount)
    {
        var account = await _context.Accounts.FindAsync(accountId);
        if (account == null)
            return false;
            
        account.Balance += amount;
        account.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<bool> IsAccountNumberUniqueAsync(string accountNumber)
    {
        return !await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber);
    }
}