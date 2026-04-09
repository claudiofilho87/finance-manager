using FinanceManager.Application.DTOs;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Interfaces;

public interface ITokenGenerator
{
    TokenResponseDto CreateTokenResponse(User user);
}
