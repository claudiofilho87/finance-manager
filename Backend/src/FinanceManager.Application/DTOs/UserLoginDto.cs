using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Application.DTOs;

public class UserLoginDto
{
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
