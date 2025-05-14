using System.ComponentModel.DataAnnotations;

namespace BankingApp.Api.DTOs;

public class RegisterDTO
{
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(13)]
    public string CNP { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;
}