using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        var userDtos = await _userService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos));
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] UserCreateDto dto)
    {
        _logger.LogInformation("POST /api/v1/users/register - Email: {Email}", dto.Email);

        var userDtoResponse = await _userService.RegisterAsync(dto);
        if (userDtoResponse == null)
        {
            _logger.LogWarning("Registration failed - Email already exists: {Email}", dto.Email);
            return BadRequest(ApiResponse<UserDto>.ErrorResponse("Email already exists"));
        }

        return Ok(ApiResponse<UserDto>.SuccessResponse(userDtoResponse, "User created successfully"));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<TokenResponseDto>>> Login(UserLoginDto dto)
    {
        _logger.LogInformation("POST /api/v1/users/login - Email: {Email}", dto.Email);

        var result = await _userService.LoginAsync(dto);
        if (result is null)
        {
            _logger.LogWarning("Login failed - Invalid credentials for email: {Email}", dto.Email);
            return BadRequest(ApiResponse<TokenResponseDto>.ErrorResponse("Invalid email or password."));
        }

        return Ok(ApiResponse<TokenResponseDto>.SuccessResponse(result, "User logged in successfully"));
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<TokenResponseDto>>> RefreshToken(RefreshTokenRequestDto dto)
    {
        _logger.LogInformation("POST /api/v1/users/refresh-token - UserId: {UserId}", dto.UserId);

        var result = await _userService.RefreshTokensAsync(dto);
        if (result is null)
        {
            _logger.LogWarning("Refresh token failed for UserId: {UserId}", dto.UserId);
            return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid refresh token."));
        }

        return Ok(ApiResponse<TokenResponseDto>.SuccessResponse(result));
    }
}