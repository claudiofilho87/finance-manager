using FinanceManager.DTOs;
using FinanceManager.Services.Interfaces;
using FinanceManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers;

[ApiController]
[Route("api/v1/bill/ocorrences")]
public class BillOcorrencesController : AuthenticatedController
{
    private readonly IBillOcorrenceService _billOcorrenceService;
    private readonly ILogger<BillOcorrencesController> _logger;

    public BillOcorrencesController(IBillOcorrenceService billOcorrenceService, ILogger<BillOcorrencesController> logger)
    {
        _billOcorrenceService = billOcorrenceService;
        _logger = logger;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BillOcorrenceDto>>> GetOcorrenceById(long id)
    {
        _logger.LogInformation("GET /api/v1/bill/ocorrences/{OccurrenceId} - UserId: {UserId}", id, UserId);

        var billOcorrenceDto = await _billOcorrenceService.GetByIdAsync(id, UserId);
        if (billOcorrenceDto == null)
        {
            _logger.LogWarning("Occurrence not found. OccurrenceId: {OccurrenceId}, UserId: {UserId}", id, UserId);
            return NotFound(ApiResponse<string>.ErrorResponse("Ocorrence not found"));
        }

        return Ok(ApiResponse<BillOcorrenceDto>.SuccessResponse(billOcorrenceDto));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BillOcorrenceDto>>>> GetAllOcorrences()
    {
        _logger.LogInformation("GET /api/v1/bill/ocorrences - UserId: {UserId}", UserId);

        var billOcorrenceDto = await _billOcorrenceService.GetAllAsync(UserId);
        return Ok(ApiResponse<IEnumerable<BillOcorrenceDto>>.SuccessResponse(billOcorrenceDto));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BillOcorrenceDto>>> CreateOcorrence([FromBody] BillOcorrenceCreateDto dto)
    {
        _logger.LogInformation("POST /api/v1/bill/ocorrences - UserId: {UserId}, BillId: {BillId}", UserId, dto.BillId);

        var billOcorrenceDto = await _billOcorrenceService.CreateAsync(dto, UserId);

        if (billOcorrenceDto == null)
        {
            _logger.LogWarning("Create occurrence failed. Invalid BillId or unauthorized. BillId: {BillId}, UserId: {UserId}", dto.BillId, UserId);
            return BadRequest(ApiResponse<string>.ErrorResponse("Invalid BillId or you are not authorized to add an occurrence to this bill."));
        }

        return CreatedAtAction(nameof(GetOcorrenceById), new { id = billOcorrenceDto.Id },
            ApiResponse<BillOcorrenceDto>.SuccessResponse(billOcorrenceDto, "Ocorrence created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BillOcorrenceDto>>> UpdateOcorrence(long id, [FromBody] BillOcorrenceCreateDto dto)
    {
        _logger.LogInformation("PUT /api/v1/bill/ocorrences/{OccurrenceId} - UserId: {UserId}", id, UserId);

        var billOcorrenceDto = await _billOcorrenceService.UpdateAsync(id, dto, UserId);
        if (billOcorrenceDto == null)
        {
            _logger.LogWarning("Update occurrence failed. Occurrence not found. OccurrenceId: {OccurrenceId}, UserId: {UserId}", id, UserId);
            return NotFound(ApiResponse<string>.ErrorResponse("Ocorrence not found"));
        }

        return Ok(ApiResponse<BillOcorrenceDto>.SuccessResponse(billOcorrenceDto, "Ocorrence updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteOcorrence(long id)
    {
        _logger.LogInformation("DELETE /api/v1/bill/ocorrences/{OccurrenceId} - UserId: {UserId}", id, UserId);

        var isDeleted = await _billOcorrenceService.DeleteAsync(id, UserId);
        if (!isDeleted)
        {
            _logger.LogWarning("Delete occurrence failed. Occurrence not found. OccurrenceId: {OccurrenceId}, UserId: {UserId}", id, UserId);
        }

        return isDeleted
            ? Ok(ApiResponse<string>.SuccessResponse("Ocorrence deleted successfully"))
            : NotFound(ApiResponse<string>.ErrorResponse("Ocorrence not found"));
    }
}