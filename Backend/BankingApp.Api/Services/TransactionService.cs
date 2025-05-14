using BankingApp.Api.Data;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Services;

public class TransactionService : ITransactionService
{
    private readonly ApplicationDbContext _context;
    private readonly IAccountRepository _accountRepository;
    
    public TransactionService(ApplicationDbContext context, IAccountRepository accountRepository)
    {
        _context = context;
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<TransactionDTO>> GetAccountTransactionsAsync(int accountId, int userId)
    {
        // Ensure the account belongs to the user
        var account = await _accountRepository.GetByIdAsync(accountId);
        if (account == null || account.UserId != userId)
            return Enumerable.Empty<TransactionDTO>();
            
        // Get all transactions where this account is either source or destination
        var transactions = await _context.Transactions
            .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
            
        return transactions.Select(t => new TransactionDTO
        {
            Id = t.Id,
            Amount = t.Amount,
            Currency = t.Currency,
            Type = t.Type,
            Note = t.Note,
            Status = t.Status,
            IsImportant = t.IsImportant,
            CreatedAt = t.CreatedAt,
            FromAccountId = t.FromAccountId,
            ToAccountId = t.ToAccountId,
            ToAccountNumber = t.ToAccountNumber,
            FromAccountNumber = t.FromAccountNumber
        });
    }
    
    public async Task<TransactionDTO?> GetTransactionByIdAsync(int transactionId, int userId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.FromAccount)
            .FirstOrDefaultAsync(t => t.Id == transactionId);
            
        if (transaction == null || transaction.FromAccount.UserId != userId)
            return null;
            
        return new TransactionDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Note = transaction.Note,
            Status = transaction.Status,
            IsImportant = transaction.IsImportant,
            CreatedAt = transaction.CreatedAt,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ToAccountNumber = transaction.ToAccountNumber,
            FromAccountNumber = transaction.FromAccountNumber
        };
    }
    
    public async Task<(bool Success, string Message, TransactionDTO? Transaction)> CreateTransactionAsync(int userId, CreateTransactionDTO createTransactionDto)
    {
        // Get the source account and validate it belongs to the user
        var fromAccount = await _accountRepository.GetByIdAsync(createTransactionDto.FromAccountId);
        if (fromAccount == null)
            return (false, "Source account not found", null);
            
        if (fromAccount.UserId != userId)
            return (false, "You don't have access to this account", null);
            
        // Check if balance is sufficient
        if (fromAccount.Balance < createTransactionDto.Amount && createTransactionDto.Type != "Deposit")
            return (false, "Insufficient balance", null);
            
        // For transfers, validate destination account
        Account? toAccount = null;
        if (createTransactionDto.Type == "Transfer" && !string.IsNullOrEmpty(createTransactionDto.ToAccountNumber))
        {
            toAccount = await _accountRepository.GetByAccountNumberAsync(createTransactionDto.ToAccountNumber);
            if (toAccount == null)
                return (false, "Destination account not found", null);
        }
        
        // Process based on transaction type
        switch (createTransactionDto.Type)
        {
            case "Transfer":
                return await ProcessTransferAsync(fromAccount, toAccount, createTransactionDto);
                
            case "Deposit":
                return await ProcessDepositAsync(fromAccount, createTransactionDto);
                
            case "Withdrawal":
                return await ProcessWithdrawalAsync(fromAccount, createTransactionDto);
                
            case "BillPayment":
                return await ProcessBillPaymentAsync(fromAccount, createTransactionDto);
                
            default:
                return (false, "Invalid transaction type", null);
        }
    }
    
    public async Task<bool> MarkTransactionAsImportantAsync(int transactionId, int userId)
    {
        var transaction = await _context.Transactions
            .Include(t => t.FromAccount)
            .FirstOrDefaultAsync(t => t.Id == transactionId);
            
        if (transaction == null || transaction.FromAccount.UserId != userId)
            return false;
            
        transaction.IsImportant = !transaction.IsImportant;
        transaction.UpdatedAt = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }
    
    private async Task<(bool Success, string Message, TransactionDTO? Transaction)> ProcessTransferAsync(
        Account fromAccount, Account? toAccount, CreateTransactionDTO createTransactionDto)
    {
        if (toAccount == null)
            return (false, "Destination account not found", null);
            
        // Check spending limit
        if (createTransactionDto.Amount > fromAccount.SpendingLimit)
            return (false, "Transaction exceeds daily spending limit", null);
            
        // Create transaction
        var transaction = new Transaction
        {
            FromAccountId = fromAccount.Id,
            ToAccountId = toAccount.Id,
            FromAccountNumber = fromAccount.AccountNumber,
            ToAccountNumber = toAccount.AccountNumber,
            Amount = createTransactionDto.Amount,
            Currency = createTransactionDto.Currency,
            Type = "Transfer",
            Note = createTransactionDto.Note,
            Status = "Completed"
        };
        
        // Update balances
        fromAccount.Balance -= createTransactionDto.Amount;
        toAccount.Balance += createTransactionDto.Amount;
        
        fromAccount.UpdatedAt = DateTime.UtcNow;
        toAccount.UpdatedAt = DateTime.UtcNow;
        
        // Save changes
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        return (true, "Transfer completed successfully", new TransactionDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Note = transaction.Note,
            Status = transaction.Status,
            IsImportant = transaction.IsImportant,
            CreatedAt = transaction.CreatedAt,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ToAccountNumber = transaction.ToAccountNumber,
            FromAccountNumber = transaction.FromAccountNumber
        });
    }
    
    private async Task<(bool Success, string Message, TransactionDTO? Transaction)> ProcessDepositAsync(
        Account account, CreateTransactionDTO createTransactionDto)
    {
        // Create transaction
        var transaction = new Transaction
        {
            FromAccountId = account.Id,
            FromAccountNumber = account.AccountNumber,
            Amount = createTransactionDto.Amount,
            Currency = createTransactionDto.Currency,
            Type = "Deposit",
            Note = createTransactionDto.Note,
            Status = "Completed"
        };
        
        // Update balance
        account.Balance += createTransactionDto.Amount;
        account.UpdatedAt = DateTime.UtcNow;
        
        // Save changes
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        return (true, "Deposit completed successfully", new TransactionDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Note = transaction.Note,
            Status = transaction.Status,
            IsImportant = transaction.IsImportant,
            CreatedAt = transaction.CreatedAt,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ToAccountNumber = transaction.ToAccountNumber,
            FromAccountNumber = transaction.FromAccountNumber
        });
    }
    
    private async Task<(bool Success, string Message, TransactionDTO? Transaction)> ProcessWithdrawalAsync(
        Account account, CreateTransactionDTO createTransactionDto)
    {
        // Check daily withdrawal limit
        if (createTransactionDto.Amount > account.DailyWithdrawalLimit)
            return (false, "Transaction exceeds daily withdrawal limit", null);
            
        // Create transaction
        var transaction = new Transaction
        {
            FromAccountId = account.Id,
            FromAccountNumber = account.AccountNumber,
            Amount = createTransactionDto.Amount,
            Currency = createTransactionDto.Currency,
            Type = "Withdrawal",
            Note = createTransactionDto.Note,
            Status = "Completed"
        };
        
        // Update balance
        account.Balance -= createTransactionDto.Amount;
        account.UpdatedAt = DateTime.UtcNow;
        
        // Save changes
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        return (true, "Withdrawal completed successfully", new TransactionDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Note = transaction.Note,
            Status = transaction.Status,
            IsImportant = transaction.IsImportant,
            CreatedAt = transaction.CreatedAt,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ToAccountNumber = transaction.ToAccountNumber,
            FromAccountNumber = transaction.FromAccountNumber
        });
    }
    
    private async Task<(bool Success, string Message, TransactionDTO? Transaction)> ProcessBillPaymentAsync(
        Account account, CreateTransactionDTO createTransactionDto)
    {
        // Check spending limit
        if (createTransactionDto.Amount > account.SpendingLimit)
            return (false, "Transaction exceeds daily spending limit", null);
            
        // Create transaction
        var transaction = new Transaction
        {
            FromAccountId = account.Id,
            FromAccountNumber = account.AccountNumber,
            ToAccountNumber = createTransactionDto.ToAccountNumber,
            Amount = createTransactionDto.Amount,
            Currency = createTransactionDto.Currency,
            Type = "BillPayment",
            Note = createTransactionDto.Note,
            Status = "Completed"
        };
        
        // Update balance
        account.Balance -= createTransactionDto.Amount;
        account.UpdatedAt = DateTime.UtcNow;
        
        // Save changes
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        
        return (true, "Bill payment completed successfully", new TransactionDTO
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Note = transaction.Note,
            Status = transaction.Status,
            IsImportant = transaction.IsImportant,
            CreatedAt = transaction.CreatedAt,
            FromAccountId = transaction.FromAccountId,
            ToAccountId = transaction.ToAccountId,
            ToAccountNumber = transaction.ToAccountNumber,
            FromAccountNumber = transaction.FromAccountNumber
        });
    }
}