using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Services;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    
    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpGet("account/{accountId}")]
    public async Task<IActionResult> GetAccountTransactions(int accountId)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var transactions = await _transactionService.GetAccountTransactionsAsync(accountId, user.Id);
        return Ok(transactions);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTransaction(int id)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var transaction = await _transactionService.GetTransactionByIdAsync(id, user.Id);
        
        if (transaction == null)
            return NotFound(new { message = "Transaction not found or you don't have access" });
            
        return Ok(transaction);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDTO createTransactionDto)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var (success, message, transaction) = await _transactionService.CreateTransactionAsync(user.Id, createTransactionDto);
        
        if (!success)
            return BadRequest(new { message });
            
        return CreatedAtAction(nameof(GetTransaction), new { id = transaction!.Id }, transaction);
    }
    
    [HttpPut("{id}/important")]
    public async Task<IActionResult> MarkAsImportant(int id)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        var success = await _transactionService.MarkTransactionAsImportantAsync(id, user.Id);
        
        if (!success)
            return NotFound(new { message = "Transaction not found or you don't have access" });
            
        return Ok(new { message = "Transaction marked as important" });
    }
}