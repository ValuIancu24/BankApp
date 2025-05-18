using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Repositories;
using BankingApp.Api.Services;
using Moq;
using Xunit;

namespace BankingApp.Tests.Services
{
    public class AccountServiceTests
    {
        [Fact]
        public async Task GetUserAccountsAsync_ReturnsUserAccounts()
        {
            // Arrange
            var userId = 1;
            var mockRepo = new Mock<IAccountRepository>();
            
            var accounts = new List<Account>
            {
                new Account 
                { 
                    Id = 1, 
                    UserId = userId, 
                    AccountNumber = "RO123456789", 
                    Balance = 1000, 
                    Currency = "RON", 
                    Status = "Active",
                    SpendingLimit = 5000,
                    DailyWithdrawalLimit = 2000,
                    CreatedAt = DateTime.UtcNow
                },
                new Account 
                { 
                    Id = 2, 
                    UserId = userId, 
                    AccountNumber = "RO987654321", 
                    Balance = 500, 
                    Currency = "USD", 
                    Status = "Active",
                    SpendingLimit = 5000,
                    DailyWithdrawalLimit = 2000,
                    CreatedAt = DateTime.UtcNow
                }
            };
            
            mockRepo.Setup(repo => repo.GetByUserIdAsync(userId))
                   .ReturnsAsync(accounts);
            
            var accountService = new AccountService(mockRepo.Object);
            
            // Act
            var result = await accountService.GetUserAccountsAsync(userId);
            
            // Assert
            var accountsList = result.ToList();
            Assert.Equal(2, accountsList.Count);
            Assert.Equal("RO123456789", accountsList[0].AccountNumber);
            Assert.Equal("RO987654321", accountsList[1].AccountNumber);
            Assert.Equal(1000, accountsList[0].Balance);
            Assert.Equal(500, accountsList[1].Balance);
        }
    }
}