using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinanceManager.DTOs;
using FinanceManager.Models;
using FinanceManager.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Services;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;
    
    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public TokenResponseDto CreateTokenResponse(User user)
    {
        return new TokenResponseDto
        {
            AccessToken = GenerateToken(user),
            RefreshToken = GenerateRefreshToken()
        };
    }
    
    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
    
    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}