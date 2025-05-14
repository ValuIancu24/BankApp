using BankingApp.Api.DTOs;

namespace BankingApp.Api.Services;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDTO>> GetAccountTransactionsAsync(int accountId, int userId);
    Task<TransactionDTO?> GetTransactionByIdAsync(int transactionId, int userId);
    Task<(bool Success, string Message, TransactionDTO? Transaction)> CreateTransactionAsync(int userId, CreateTransactionDTO createTransactionDto);
    Task<bool> MarkTransactionAsImportantAsync(int transactionId, int userId);
}