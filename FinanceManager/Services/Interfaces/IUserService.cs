using FinanceManager.DTOs;

namespace FinanceManager.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<UserDto>> GetAllAsync();
    public Task<UserDto?> RegisterAsync(UserCreateDto dto);
    public Task<TokenResponseDto?> LoginAsync(UserLoginDto dto);
    public Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto dto);
}