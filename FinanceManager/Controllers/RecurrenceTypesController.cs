using FinanceManager.DTOs;
using FinanceManager.Services.Interfaces;
using FinanceManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers;

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

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApiResponse<RecurrenceTypeDto>>> CreateRecurrence(
        [FromBody] RecurrenceTypeCreateDto dto)
    {
        _logger.LogInformation("POST /api/v1/recurrence-types - UserId: {UserId}", UserId);

        var recurrenceDto = await _recurrenceService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetRecurrenceById), new { id = recurrenceDto.Id },
            ApiResponse<RecurrenceTypeDto>.SuccessResponse(recurrenceDto, "Recurrence Type created successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<RecurrenceTypeDto>>> UpdateRecurrence(long id,
        [FromBody] RecurrenceTypeCreateDto dto)
    {
        _logger.LogInformation("PUT /api/v1/recurrence-types/{RecurrenceTypeId} - UserId: {UserId}", id, UserId);

        var recurrenceDto = await _recurrenceService.UpdateAsync(id, dto);
        if (recurrenceDto == null)
        {
            _logger.LogWarning("Update recurrence type failed. RecurrenceType not found. RecurrenceTypeId: {RecurrenceTypeId}", id);
            return NotFound(ApiResponse<string>.ErrorResponse("Recurrence Type not found"));
        }

        return Ok(ApiResponse<RecurrenceTypeDto>.SuccessResponse(recurrenceDto,
            "Recurrence Type updated successfully"));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteRecurrence(long id)
    {
        _logger.LogInformation("DELETE /api/v1/recurrence-types/{RecurrenceTypeId} - UserId: {UserId}", id, UserId);

        var isDeleted = await _recurrenceService.DeleteAsync(id);
        if (!isDeleted)
        {
            _logger.LogWarning("Delete recurrence type failed. RecurrenceType not found. RecurrenceTypeId: {RecurrenceTypeId}", id);
        }

        return isDeleted
            ? Ok(ApiResponse<string>.SuccessResponse("Recurrence Type deleted successfully"))
            : NotFound(ApiResponse<string>.ErrorResponse("Recurrence Type not found"));
    }
}