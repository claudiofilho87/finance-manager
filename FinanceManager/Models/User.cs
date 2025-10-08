using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Models;

[Table("users")]
public class User
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("email")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Column("username")]
    [StringLength(255)]
    public string Username { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Column("role")]
    [StringLength(255)]
    public string Role { get; set; } = string.Empty;
    
    [Column("refresh_token")]
    public string? RefreshToken { get; set; }
    
    [Column("refresh_token_expiry_time")]
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}