using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> RegisterAsync(UserCreateDto dto);
    Task<TokenResponseDto?> LoginAsync(UserLoginDto dto);
    Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto dto);
}
