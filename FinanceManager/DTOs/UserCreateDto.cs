using System.ComponentModel.DataAnnotations;

namespace FinanceManager.DTOs;

public class UserCreateDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(255)]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
    public string Password { get; set; } = string.Empty;
    
    public string? Role { get; set; }
}