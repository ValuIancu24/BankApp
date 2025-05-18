using System;
using System.Threading.Tasks;
using BankingApp.Api.Data;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Repositories;
using BankingApp.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using System.Collections.Generic;

namespace BankingApp.Tests.Services
{
    public class TransactionServiceTests
    {
        [Fact]
        public async Task CreateTransactionAsync_WithValidDeposit_ReturnsSuccessAndTransaction()
        {
            // Arrange
            var userId = 1;
            var accountId = 1;
            
            var createTransactionDto = new CreateTransactionDTO
            {
                FromAccountId = accountId,
                Amount = 100.00m,
                Currency = "RON",
                Type = "Deposit",
                Note = "Test deposit"
            };
            
            var account = new Account
            {
                Id = accountId,
                UserId = userId,
                Balance = 500.00m,
                Currency = "RON",
                AccountNumber = "RO123456789",
                Status = "Active",
                SpendingLimit = 5000m,
                DailyWithdrawalLimit = 2000m,
                CreatedAt = DateTime.UtcNow
            };
            
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestTransactionDb")
                .Options;
                
            // Seed the database with test account
            using (var context = new ApplicationDbContext(options))
            {
                context.Accounts.Add(account);
                await context.SaveChangesAsync();
            }
            
            // We need a real account repository instead of a mock
            using (var context = new ApplicationDbContext(options))
            {
                var accountRepository = new AccountRepository(context);
                var transactionService = new TransactionService(context, accountRepository);
                
                // Act
                var result = await transactionService.CreateTransactionAsync(userId, createTransactionDto);
                
                // Assert
                Assert.True(result.Success);
                Assert.NotNull(result.Transaction);
                Assert.Equal("Deposit", result.Transaction.Type);
                Assert.Equal(100.00m, result.Transaction.Amount);
                
                // Also verify the account balance was updated
                var updatedAccount = await accountRepository.GetByIdAsync(accountId);
                Assert.Equal(600.00m, updatedAccount?.Balance ?? 0);
            }
        }
    }
}