using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Services;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserAccounts()
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var accounts = await _accountService.GetUserAccountsAsync(user.Id);
        return Ok(accounts);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetAccount(int id)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var account = await _accountService.GetAccountByIdAsync(id, user.Id);
        
        if (account == null)
            return NotFound(new { message = "Account not found" });
            
        return Ok(account);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDTO createAccountDto)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var account = await _accountService.CreateAccountAsync(user.Id, createAccountDto);
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }
    
    [HttpPut("{id}/spending-limit")]
    public async Task<IActionResult> UpdateSpendingLimit(int id, [FromBody] UpdateSpendingLimitDTO updateDto)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var success = await _accountService.UpdateSpendingLimitAsync(id, user.Id, updateDto.NewLimit);
        
        if (!success)
            return NotFound(new { message = "Account not found or you don't have access" });
            
        return Ok(new { message = "Spending limit updated successfully" });
    }
}