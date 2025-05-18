using System;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Api.Data;
using BankingApp.Api.Models;
using BankingApp.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BankingApp.Tests.Repositories
{
    public class TransactionRepositoryTests
    {
        private DbContextOptions<ApplicationDbContext> GetDbContextOptions(string dbName)
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
        }
        
        [Fact]
        public async Task GetByAccountIdAsync_ReturnsTransactionsForAccount()
        {
            // Arrange
            var options = GetDbContextOptions("GetByAccountIdAsync_Test");
            var accountId = 1;
            
            // Seed the database
            using (var context = new ApplicationDbContext(options))
            {
                // Create a user
                var user = new User
                {
                    Id = 1,
                    Username = "testuser",
                    Email = "test@example.com",
                    PasswordHash = "hash",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CNP = "1234567890123",
                    FirstName = "Test",
                    LastName = "User",
                    City = "TestCity",
                    BirthDate = DateTime.UtcNow.AddYears(-30),
                    PhoneNumber = "123456789"
                };
                
                // Create accounts
                var account1 = new Account
                {
                    Id = accountId,
                    UserId = user.Id,
                    AccountNumber = "RO123456789",
                    Balance = 1000,
                    Currency = "RON",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    SpendingLimit = 5000,
                    DailyWithdrawalLimit = 2000
                };
                
                var account2 = new Account
                {
                    Id = 2,
                    UserId = user.Id,
                    AccountNumber = "RO987654321",
                    Balance = 500,
                    Currency = "USD",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    SpendingLimit = 5000,
                    DailyWithdrawalLimit = 2000
                };
                
                // Create transactions
                var transaction1 = new Transaction
                {
                    Id = 1,
                    FromAccountId = accountId,
                    Amount = 100,
                    Currency = "RON",
                    Type = "Deposit",
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow,
                    FromAccount = account1
                };
                
                var transaction2 = new Transaction
                {
                    Id = 2,
                    FromAccountId = accountId,
                    ToAccountId = 2,
                    Amount = 50,
                    Currency = "RON",
                    Type = "Transfer",
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow,
                    FromAccount = account1,
                    ToAccount = account2
                };
                
                var transaction3 = new Transaction
                {
                    Id = 3,
                    FromAccountId = 2,
                    Amount = 25,
                    Currency = "USD",
                    Type = "Withdrawal",
                    Status = "Completed",
                    CreatedAt = DateTime.UtcNow,
                    FromAccount = account2
                };
                
                context.Users.Add(user);
                context.Accounts.AddRange(account1, account2);
                context.Transactions.AddRange(transaction1, transaction2, transaction3);
                await context.SaveChangesAsync();
            }
            
            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TransactionRepository(context);
                var result = await repository.GetByAccountIdAsync(accountId);
                
                // Assert
                var transactions = result.ToList();
                Assert.Equal(2, transactions.Count);
                Assert.Contains(transactions, t => t.Id == 1);
                Assert.Contains(transactions, t => t.Id == 2);
                Assert.DoesNotContain(transactions, t => t.Id == 3);
            }
        }
        
        [Fact]
        public async Task MarkAsImportantAsync_TogglesImportantFlag()
        {
            // Arrange
            var options = GetDbContextOptions("MarkAsImportantAsync_Test");
            var transactionId = 1;
            
            // Seed the database
            using (var context = new ApplicationDbContext(options))
            {
                // Create account
                var account = new Account
                {
                    Id = 1,
                    UserId = 1,
                    AccountNumber = "RO123456789",
                    Balance = 1000,
                    Currency = "RON",
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow,
                    SpendingLimit = 5000,
                    DailyWithdrawalLimit = 2000
                };
                
                // Create transaction
                var transaction = new Transaction
                {
                    Id = transactionId,
                    FromAccountId = account.Id,
                    Amount = 100,
                    Currency = "RON",
                    Type = "Deposit",
                    Status = "Completed",
                    IsImportant = false,
                    CreatedAt = DateTime.UtcNow,
                    FromAccount = account
                };
                
                context.Accounts.Add(account);
                context.Transactions.Add(transaction);
                await context.SaveChangesAsync();
            }
            
            // Act
            using (var context = new ApplicationDbContext(options))
            {
                var repository = new TransactionRepository(context);
                
                // Mark transaction as important
                bool result = await repository.MarkAsImportantAsync(transactionId);
                
                // Verify the transaction was marked as important
                var transaction = await context.Transactions.FindAsync(transactionId);
                
                // Assert
                Assert.True(result);
                Assert.NotNull(transaction);
                Assert.True(transaction!.IsImportant);
                Assert.NotNull(transaction.UpdatedAt);
            }
        }
    }
}