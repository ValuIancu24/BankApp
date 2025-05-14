using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.DTOs;
using BankingApp.Api.Services;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public AuthController(IAuthService authService)
    {
        _authService = authService;
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
}