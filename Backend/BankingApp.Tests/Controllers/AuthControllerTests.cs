using System.Threading.Tasks;
using BankingApp.Api.Controllers;
using BankingApp.Api.DTOs;
using BankingApp.Api.Helpers;
using BankingApp.Api.Models;
using BankingApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using System.Text.Json;

namespace BankingApp.Tests.Controllers
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOkWithTokenAndUser()
        {
            // Arrange
            var loginDto = new LoginDTO
            {
                Username = "testuser",
                Password = "Password123!"
            };
            
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                IsActive = true
            };
            
            var token = "test-jwt-token";
            
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.LoginAsync(loginDto))
                          .ReturnsAsync((user, token));
            
            var mockConfig = new Mock<IConfiguration>();
            var mockJwtHelper = new Mock<JwtHelper>(mockConfig.Object);
            
            var controller = new AuthController(mockAuthService.Object, mockJwtHelper.Object);
            
            // Act
            var result = await controller.Login(loginDto);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            
            // Use serialization to check the structure
            var json = JsonSerializer.Serialize(okResult.Value);
            Assert.Contains(token, json);
            Assert.Contains(user.Id.ToString(), json);
        }
        
        [Fact]
        public async Task Register_WithValidData_ReturnsOkWithSuccessMessage()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Username = "newuser",
                Password = "Password123!",
                FirstName = "New",
                LastName = "User",
                Email = "new@example.com",
                CNP = "1234567890123",
                City = "TestCity",
                BirthDate = new System.DateTime(1990, 1, 1),
                PhoneNumber = "0123456789"
            };
            
            var expectedMessage = "Registration successful. Please wait for admin activation.";
            
            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(service => service.RegisterAsync(registerDto))
                          .ReturnsAsync((true, expectedMessage));
            
            var mockConfig = new Mock<IConfiguration>();
            var mockJwtHelper = new Mock<JwtHelper>(mockConfig.Object);
            
            var controller = new AuthController(mockAuthService.Object, mockJwtHelper.Object);
            
            // Act
            var result = await controller.Register(registerDto);
            
            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var json = JsonSerializer.Serialize(okResult.Value);
            Assert.Contains(expectedMessage, json);
        }
    }
}