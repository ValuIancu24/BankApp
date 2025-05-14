using Microsoft.AspNetCore.Mvc;
using BankingApp.Api.DTOs;
using BankingApp.Api.Models;
using BankingApp.Api.Services;

namespace BankingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillPaymentController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    
    public BillPaymentController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }
    
    [HttpPost]
    public async Task<IActionResult> PayBill([FromBody] BillPaymentDTO billPaymentDto)
    {
        var user = HttpContext.Items["User"] as User;
        if (user == null)
            return Unauthorized(new { message = "You must be logged in" });
            
        // Create transaction for bill payment
        var createTransactionDto = new CreateTransactionDTO
        {
            FromAccountId = billPaymentDto.AccountId,
            Amount = billPaymentDto.Amount,
            Currency = billPaymentDto.Currency,
            Type = "BillPayment",
            Note = $"{billPaymentDto.BillType} bill - {billPaymentDto.Provider} - {billPaymentDto.BillNumber}{(string.IsNullOrEmpty(billPaymentDto.Note) ? "" : $" - {billPaymentDto.Note}")}",
            ToAccountNumber = $"BILL-{billPaymentDto.Provider.Replace(" ", "")}-{billPaymentDto.BillNumber}"
        };
        
        var (success, message, transaction) = await _transactionService.CreateTransactionAsync(user.Id, createTransactionDto);
        
        if (!success)
            return BadRequest(new { message });
            
        return Ok(new { message = "Bill payment successful", transaction });
    }
}