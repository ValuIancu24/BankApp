using System.Collections.Generic;
using System.Threading.Tasks;
using BankingApp.Api.Controllers;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;

namespace BankingApp.Tests.Controllers
{
    public class TransactionControllerTests
    {
        [Fact]
        public async Task GetAccountTransactions_WithValidAccountAndUser_ReturnsOkWithTransactions()
        {
            // Arrange
            var userId = 1;
            var accountId = 2;
            var user = new User { Id = userId, Username = "testuser" };
            
            var mockTransactionService = new Mock<ITransactionService>();
            var transactions = new List<TransactionDTO>
            {
                new TransactionDTO
                {
                    Id = 1,
                    FromAccountId = accountId,
                    Amount = 100.00m,
                    Currency = "RON",
                    Type = "Deposit",
                    Status = "Completed"
                }
            };
            
            mockTransactionService.Setup(service => service.GetAccountTransactionsAsync(accountId, userId))
                                .ReturnsAsync(transactions);
            
            var controller = new TransactionController(mockTransactionService.Object);
            
            // Set up HttpContext with authenticated user
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Items = { ["User"] = user }
                }
            };
            
            // Act
            var result = await controller.GetAccountTransactions(accountId);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTransactions = Assert.IsAssignableFrom<IEnumerable<TransactionDTO>>(okResult.Value);
            Assert.Single(returnedTransactions);
        }
        
        [Fact]
        public async Task CreateTransaction_WithValidData_ReturnsCreatedWithTransaction()
        {
            // Arrange
            var userId = 1;
            var user = new User { Id = userId, Username = "testuser" };
            
            var createTransactionDto = new CreateTransactionDTO
            {
                FromAccountId = 2,
                Amount = 100.00m,
                Currency = "RON",
                Type = "Deposit"
            };
            
            var createdTransaction = new TransactionDTO
            {
                Id = 1,
                FromAccountId = 2,
                Amount = 100.00m,
                Currency = "RON",
                Type = "Deposit",
                Status = "Completed"
            };
            
            var mockTransactionService = new Mock<ITransactionService>();
            mockTransactionService.Setup(service => service.CreateTransactionAsync(userId, createTransactionDto))
                                .ReturnsAsync((true, "Transaction successful", createdTransaction));
            
            var controller = new TransactionController(mockTransactionService.Object);
            
            // Set up HttpContext with authenticated user
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Items = { ["User"] = user }
                }
            };
            
            // Set up URL helper that can actually handle the CreatedAtAction result
            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock
                .Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("testUrl");
            controller.Url = urlHelperMock.Object;
            
            // Act
            var result = await controller.CreateTransaction(createTransactionDto);
            
            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(controller.GetTransaction), createdResult.ActionName);
            Assert.Equal(1, createdResult.RouteValues?["id"]);
            
            var returnedTransaction = Assert.IsType<TransactionDTO>(createdResult.Value);
            Assert.Equal(createdTransaction.Id, returnedTransaction.Id);
        }
    }
}