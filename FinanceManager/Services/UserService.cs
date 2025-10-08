using FinanceManager.Data;
using FinanceManager.DTOs;
using FinanceManager.Models;
using FinanceManager.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordHasher<User> _passwordHasher;
    
    public UserService(AppDbContext context, IPasswordHasher<User> passwordHasher, ITokenGenerator tokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(user => new UserDto
            {
                Email = user.Email,
                Username = user.Username,
                Role = user.Role
            })
            .ToListAsync();
    }

    public async Task<UserDto?> RegisterAsync(UserCreateDto dto)
    {
        var userExists = await _context.Users.AnyAsync(user => user.Email == dto.Email);
        if (userExists) return null;
        
        var now = DateTime.UtcNow;
        var user = new User
        {
            Email = dto.Email.Trim(),
            Username = dto.Username.Trim(),
            Role = string.IsNullOrWhiteSpace(dto.Role) ? "User" : dto.Role,
            CreatedAt = now,
            UpdatedAt = now
        };
        
        var passwordHash = _passwordHasher.HashPassword(user, dto.Password);
        user.PasswordHash = passwordHash;
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return new UserDto
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        };
    }

    public async Task<TokenResponseDto?> LoginAsync(UserLoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == dto.Email);
        if (user == null) return null;
        
        var verifyPasswordHash = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (verifyPasswordHash == PasswordVerificationResult.Failed) return null;
        
        var tokenResponse = _tokenGenerator.CreateTokenResponse(user);

        user.RefreshToken = tokenResponse.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        await _context.SaveChangesAsync();
        
        return tokenResponse;
    }
    
    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto dto)
    {
        var user = await ValidateRefreshTokenAsync(dto.UserId, dto.RefreshToken);
        if (user is null)
            return null;

        var tokenResponse = _tokenGenerator.CreateTokenResponse(user);
        tokenResponse.RefreshToken = dto.RefreshToken;
        
        return tokenResponse;
    }
    
    private async Task<User?> ValidateRefreshTokenAsync(long userId, string refreshToken)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user is null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }
}