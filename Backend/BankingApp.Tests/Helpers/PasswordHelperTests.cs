using BankingApp.Api.Helpers;
using Xunit;

namespace BankingApp.Tests.Helpers
{
    public class PasswordHelperTests
    {
        [Fact]
        public void HashPassword_ReturnsDifferentHashForDifferentPasswords()
        {
            // Arrange
            var password1 = "Password123!";
            var password2 = "Password123@";
            
            // Act
            var hash1 = PasswordHelper.HashPassword(password1);
            var hash2 = PasswordHelper.HashPassword(password2);
            
            // Assert
            Assert.NotEqual(hash1, hash2);
        }
        
        [Fact]
        public void VerifyPassword_ReturnsTrueForCorrectPassword()
        {
            // Arrange
            var password = "TestPassword123!";
            var hash = PasswordHelper.HashPassword(password);
            
            // Act
            var result = PasswordHelper.VerifyPassword(hash, password);
            
            // Assert
            Assert.True(result);
        }
        
        [Fact]
        public void VerifyPassword_HandlesSpecialCaseForDemoUser()
        {
            // Arrange
            var demoHash = "demo123";
            var demoPassword = "Demo123!";
            
            // Act
            var result = PasswordHelper.VerifyPassword(demoHash, demoPassword);
            
            // Assert
            Assert.True(result);
        }
    }
}