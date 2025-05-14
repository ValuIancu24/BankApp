using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BankingApp.Api.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "text")]
    public string PasswordHash { get; set; } = string.Empty;
    
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
    public string CNP { get; set; } = string.Empty; // Romanian personal identification number
    
    [MaxLength(50)]
    public string City { get; set; } = string.Empty;
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    public bool IsActive { get; set; } = false; // Account needs to be activated by admin
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}