using BankingApp.Api.Data;
using BankingApp.Api.DTOs;
using BankingApp.Api.Helpers;
using BankingApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;
    
    public AuthService(ApplicationDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }
    
    public async Task<(User? User, string? Token)> LoginAsync(LoginDTO loginDto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == loginDto.Username);
            
        if (user == null)
            return (null, null);
            
        if (!PasswordHelper.VerifyPassword(user.PasswordHash, loginDto.Password))
            return (null, null);
            
        if (!user.IsActive)
            return (null, null);
            
        var token = _jwtHelper.GenerateToken(user);
        
        return (user, token);
    }
    
    public async Task<(bool Success, string Message)> RegisterAsync(RegisterDTO registerDto)
    {
        // Check if username is already taken
        if (await _context.Users.AnyAsync(u => u.Username == registerDto.Username))
            return (false, "Username is already taken");
            
        // Check if email is already taken
        if (await _context.Users.AnyAsync(u => u.Email == registerDto.Email))
            return (false, "Email is already taken");
            
        // Check if CNP is already registered
        if (await _context.Users.AnyAsync(u => u.CNP == registerDto.CNP))
            return (false, "CNP is already registered");
            
        // Create new user
        var user = new User
        {
            Username = registerDto.Username,
            PasswordHash = PasswordHelper.HashPassword(registerDto.Password),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            CNP = registerDto.CNP,
            City = registerDto.City,
            BirthDate = registerDto.BirthDate,
            PhoneNumber = registerDto.PhoneNumber,
            IsActive = false // User needs to be activated by admin
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        return (true, "Registration successful. Please wait for admin activation.");
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}