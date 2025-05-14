using BankingApp.Api.DTOs;
using BankingApp.Api.Models;

namespace BankingApp.Api.Services;

public interface IAuthService
{
    Task<(User? User, string? Token)> LoginAsync(LoginDTO loginDto);
    Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDto);
    Task<User?> GetUserByIdAsync(int id);
}