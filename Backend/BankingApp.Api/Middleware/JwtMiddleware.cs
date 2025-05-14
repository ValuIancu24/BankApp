using System.IdentityModel.Tokens.Jwt;
using System.Text;
using BankingApp.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BankingApp.Api.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    
    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }
    
    public async Task Invoke(HttpContext context, ApplicationDbContext dbContext)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (token != null)
        {
            Console.WriteLine($"JWT Token received: {token.Substring(0, Math.Min(20, token.Length))}...");
            await AttachUserToContext(context, dbContext, token);
        }
        else
        {
            // Log when no token is found
            Console.WriteLine($"No JWT token in request to {context.Request.Path}");
        }
            
        await _next(context);
    }
    
    private async Task AttachUserToContext(HttpContext context, ApplicationDbContext dbContext, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is not configured"));
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "nameid").Value);
            
            // Attach user to context
            var user = await dbContext.Users
                .Include(u => u.Accounts)
                .FirstOrDefaultAsync(u => u.Id == userId);
                
            if (user != null)
            {
                context.Items["User"] = user;
                Console.WriteLine($"User authenticated: {user.Username} (ID: {user.Id})");
            }
            else
            {
                Console.WriteLine($"User with ID {userId} not found in database");
            }
        }
        catch (Exception ex)
        {
            // Log validation errors
            Console.WriteLine($"Token validation failed: {ex.Message}");
        }
    }
}