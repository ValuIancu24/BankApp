using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.DTOs;
using BankingApp.Api.Services;
using BankingApp.Api.Helpers;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly JwtHelper _jwtHelper;
    
    public AuthController(IAuthService authService, JwtHelper jwtHelper)
    {
        _authService = authService;
        _jwtHelper = jwtHelper;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var (user, token) = await _authService.LoginAsync(loginDto);
        
        if (user == null || token == null)
            return Unauthorized(new { message = "Invalid username or password" });
            
        return Ok(new { 
            token, 
            user = new {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                isActive = user.IsActive
            } 
        });
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
    {
        var (success, message) = await _authService.RegisterAsync(registerDto);
        
        if (!success)
            return BadRequest(new { message });
            
        return Ok(new { message });
    }
    
    // Development-only endpoint to bypass password verification
    [HttpPost("devlogin")]
    public async Task<IActionResult> DevLogin()
    {
        // Find demo user
        var user = await _authService.GetUserByIdAsync(1);
        
        if (user == null)
            return Unauthorized(new { message = "Demo user not found" });
            
        var token = _jwtHelper.GenerateToken(user);
            
        return Ok(new { 
            token, 
            user = new {
                id = user.Id,
                username = user.Username,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                isActive = user.IsActive,
                phoneNumber = user.PhoneNumber,
                city = user.City,
            } 
        });
    }
}