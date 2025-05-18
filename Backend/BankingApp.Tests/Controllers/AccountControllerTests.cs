using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.Api.Controllers;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Text.Json;

namespace BankingApp.Tests.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task GetAccount_WithValidIdAndUser_ReturnsOkWithAccount()
        {
            // Arrange
            var userId = 1;
            var accountId = 2;
            var user = new User { Id = userId, Username = "testuser" };
            
            var mockAccountService = new Mock<IAccountService>();
            var account = new AccountDTO
            {
                Id = accountId,
                AccountNumber = "RO123456789",
                Balance = 1000,
                Currency = "RON",
                Status = "Active"
            };
            
            mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId, userId))
                             .ReturnsAsync(account);
            
            var controller = new AccountController(mockAccountService.Object);
            
            // Set up HttpContext with authenticated user
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Items = { ["User"] = user }
                }
            };
            
            // Act
            var result = await controller.GetAccount(accountId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedAccount = Assert.IsType<AccountDTO>(okResult.Value);
            Assert.Equal(accountId, returnedAccount.Id);
            Assert.Equal("RO123456789", returnedAccount.AccountNumber);
        }

        [Fact]
        public async Task GetAccount_WithNonexistentAccount_ReturnsNotFound()
        {
            // Arrange
            var userId = 1;
            var accountId = 99; // Non-existent account
            var user = new User { Id = userId, Username = "testuser" };
            
            var mockAccountService = new Mock<IAccountService>();
            
            // Setup to return null (account not found)
            mockAccountService.Setup(service => service.GetAccountByIdAsync(accountId, userId))
                             .ReturnsAsync((AccountDTO?)null);
            
            var controller = new AccountController(mockAccountService.Object);
            
            // Set up HttpContext with authenticated user
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Items = { ["User"] = user }
                }
            };
            
            // Act
            var result = await controller.GetAccount(accountId);
            
            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            // Instead of using dynamic, check the actual structure of the object
            var resultValue = notFoundResult.Value as object;
            // You'll need to examine what structure your controller returns
            // typically something like new { message = "Account not found" }
            var json = JsonSerializer.Serialize(resultValue);
            Assert.Contains("Account not found", json);
        }
    }
}