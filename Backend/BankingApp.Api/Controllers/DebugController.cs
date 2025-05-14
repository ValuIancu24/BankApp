using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.Data;
using BankingApp.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly JwtHelper _jwtHelper;
    
    public DebugController(ApplicationDbContext context, JwtHelper jwtHelper)
    {
        _context = context;
        _jwtHelper = jwtHelper;
    }
    
    [HttpGet("testauth")]
    public async Task<IActionResult> TestAuth()
    {
        var users = await _context.Users.ToListAsync();
        
        var testUser = users.FirstOrDefault(u => u.Username == "demo");
        
        if (testUser == null)
            return NotFound(new { message = "Demo user not found" });
            
        var canVerify = PasswordHelper.VerifyPassword(testUser.PasswordHash, "Demo123!");
        
        var sampleToken = _jwtHelper.GenerateToken(testUser);
        
        return Ok(new { 
            userCount = users.Count,
            demoUserExists = testUser != null,
            demoUserName = testUser?.Username,
            demoUserHash = testUser?.PasswordHash,
            passwordVerifies = canVerify,
            isActive = testUser?.IsActive,
            sampleToken = sampleToken
        });
    }
}