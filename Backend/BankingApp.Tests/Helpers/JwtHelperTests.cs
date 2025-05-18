using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using BankingApp.Api.Helpers;
using BankingApp.Api.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BankingApp.Tests.Helpers
{
    public class JwtHelperTests
    {
        [Fact]
        public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "test@example.com"
            };
            
            // Fix the nullability warning by using the correct Key-Value type
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                {"Jwt:Key", "ThisIsAVeryLongSecretKeyForTestingPurposesOnly"},
                {"Jwt:Issuer", "TestIssuer"},
                {"Jwt:Audience", "TestAudience"}
            });
            
            IConfiguration configuration = configBuilder.Build();
            
            var jwtHelper = new JwtHelper(configuration);
            
            // Act
            var token = jwtHelper.GenerateToken(user);
            
            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
            
            // Additional validation - decode the token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            // Check that it contains expected claims
            var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            Assert.NotNull(nameIdClaim);
            Assert.Equal(user.Id.ToString(), nameIdClaim?.Value);
            
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name");
            Assert.NotNull(nameClaim);
            Assert.Equal(user.Username, nameClaim?.Value);
        }
    }
}