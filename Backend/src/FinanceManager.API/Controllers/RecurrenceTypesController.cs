using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/v1/recurrence-types")]
public class RecurrenceTypesController : AuthenticatedController
{
    private readonly IRecurrenceTypeService _recurrenceService;
    private readonly ILogger<RecurrenceTypesController> _logger;

    public RecurrenceTypesController(IRecurrenceTypeService recurrenceService, ILogger<RecurrenceTypesController> logger)
    {
        _recurrenceService = recurrenceService;
        _logger = logger;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<RecurrenceTypeDto>>> GetRecurrenceById(long id)
    {
        _logger.LogInformation("GET /api/v1/recurrence-types/{RecurrenceTypeId} - UserId: {UserId}", id, UserId);

        var recurrenceDto = await _recurrenceService.GetByIdAsync(id);
        if (recurrenceDto == null)
        {
            _logger.LogWarning("Recurrence type not found. RecurrenceTypeId: {RecurrenceTypeId}", id);
            return NotFound(ApiResponse<string>.ErrorResponse("Recurrence Type not found"));
        }

        return Ok(ApiResponse<RecurrenceTypeDto>.SuccessResponse(recurrenceDto));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<RecurrenceTypeDto>>>> GetAllRecurrences()
    {
        _logger.LogInformation("GET /api/v1/recurrence-types - UserId: {UserId}", UserId);

        var recurrenceDtos = await _recurrenceService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<RecurrenceTypeDto>>.SuccessResponse(recurrenceDtos));
    }

}