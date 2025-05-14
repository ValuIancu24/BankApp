using BankingApp.Api.Data;
using BankingApp.Api.Helpers;
using BankingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Data;

public class DataSeeder
{
    public static async Task SeedData(ApplicationDbContext context)
    {
        // Check if data already exists
        if (await context.Users.AnyAsync())
            return; // DB has been seeded already
            
        // Create users
        var demoUser = new User
        {
            Username = "demo",
            PasswordHash = PasswordHelper.HashPassword("Demo123!"),
            FirstName = "Demo",
            LastName = "User",
            Email = "demo@example.com",
            CNP = "1234567890123",
            City = "Bucharest",
            BirthDate = DateTime.UtcNow.AddYears(-30),
            PhoneNumber = "0712345678",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        var adminUser = new User
        {
            Username = "admin",
            PasswordHash = PasswordHelper.HashPassword("Admin123!"),
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@example.com",
            CNP = "9876543210123",
            City = "Craiova",
            BirthDate = DateTime.UtcNow.AddYears(-40),
            PhoneNumber = "0723456789",
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        await context.Users.AddRangeAsync(demoUser, adminUser);
        await context.SaveChangesAsync();
        
        // Create accounts for demo user
        var ronAccount = new Account
        {
            AccountNumber = "RO49BANK10002000300001",
            Balance = 5000m,
            Currency = "RON",
            SpendingLimit = 2000m,
            DailyWithdrawalLimit = 1000m,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UserId = demoUser.Id
        };
        
        var eurAccount = new Account
        {
            AccountNumber = "RO49BANK10002000300002",
            Balance = 1000m,
            Currency = "EUR",
            SpendingLimit = 1500m,
            DailyWithdrawalLimit = 800m,
            Status = "Active",
            CreatedAt = DateTime.UtcNow,
            UserId = demoUser.Id
        };
        
        await context.Accounts.AddRangeAsync(ronAccount, eurAccount);
        await context.SaveChangesAsync();
        
        // Create some sample transactions
        var transactions = new List<Transaction>
        {
            new Transaction
            {
                FromAccountId = ronAccount.Id,
                FromAccountNumber = ronAccount.AccountNumber,
                Amount = 500m,
                Currency = "RON",
                Type = "Deposit",
                Note = "Initial deposit",
                Status = "Completed",
                IsImportant = false,
                CreatedAt = DateTime.UtcNow.AddDays(-5)
            },
            new Transaction
            {
                FromAccountId = ronAccount.Id,
                FromAccountNumber = ronAccount.AccountNumber,
                Amount = 200m,
                Currency = "RON",
                Type = "Withdrawal",
                Note = "ATM withdrawal",
                Status = "Completed",
                IsImportant = false,
                CreatedAt = DateTime.UtcNow.AddDays(-3)
            },
            new Transaction
            {
                FromAccountId = ronAccount.Id,
                FromAccountNumber = ronAccount.AccountNumber,
                ToAccountNumber = "RO49BANK99998888777766",
                Amount = 350m,
                Currency = "RON",
                Type = "Transfer",
                Note = "Payment to John",
                Status = "Completed",
                IsImportant = true,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Transaction
            {
                FromAccountId = eurAccount.Id,
                FromAccountNumber = eurAccount.AccountNumber,
                Amount = 100m,
                Currency = "EUR",
                Type = "Deposit",
                Note = "Initial deposit",
                Status = "Completed",
                IsImportant = false,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            }
        };
        
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
        
        Console.WriteLine("Database seeded successfully.");
    }
}