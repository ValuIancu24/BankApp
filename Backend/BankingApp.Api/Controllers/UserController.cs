using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.Models;
using BankingApp.Api.Services;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;
    
    public UserController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        return Ok(new {
            id = user.Id,
            username = user.Username,
            firstName = user.FirstName,
            lastName = user.LastName,
            email = user.Email,
            city = user.City,
            phoneNumber = user.PhoneNumber,
            isActive = user.IsActive
        });
    }
}