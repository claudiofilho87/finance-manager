using FinanceManager.DTOs;
using FinanceManager.Models;

namespace FinanceManager.Services.Interfaces;

public interface ITokenGenerator
{
    public TokenResponseDto CreateTokenResponse(User user);
}