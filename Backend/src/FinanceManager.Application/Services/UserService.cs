using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Interfaces;

namespace FinanceManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IPasswordService _passwordService;

    public UserService(IUserRepository userRepository, IPasswordService passwordService, ITokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(user => new UserDto
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        });
    }

    public async Task<UserDto?> RegisterAsync(UserCreateDto dto)
    {
        var userExists = await _userRepository.ExistsByEmailAsync(dto.Email);
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

        user.PasswordHash = _passwordService.HashPassword(user, dto.Password);

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return new UserDto
        {
            Email = user.Email,
            Username = user.Username,
            Role = user.Role
        };
    }

    public async Task<TokenResponseDto?> LoginAsync(UserLoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null) return null;

        var isValidPassword = _passwordService.VerifyPassword(user, user.PasswordHash, dto.Password);
        if (!isValidPassword) return null;

        var tokenResponse = _tokenGenerator.CreateTokenResponse(user);

        user.RefreshToken = tokenResponse.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        await _userRepository.SaveChangesAsync();

        return tokenResponse;
    }

    public async Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto dto)
    {
        var user = await ValidateRefreshTokenAsync(dto.UserId, dto.RefreshToken);
        if (user is null) return null;

        var tokenResponse = _tokenGenerator.CreateTokenResponse(user);

        user.RefreshToken = tokenResponse.RefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
        await _userRepository.SaveChangesAsync();

        return tokenResponse;
    }

    private async Task<User?> ValidateRefreshTokenAsync(long userId, string refreshToken)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return null;
        }

        return user;
    }
}
