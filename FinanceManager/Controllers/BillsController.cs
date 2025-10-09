using FinanceManager.DTOs;
using FinanceManager.Services.Interfaces;
using FinanceManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Controllers;

[ApiController]
[Route("api/v1/bills")]
public class BillsController : AuthenticatedController
{
    private readonly IBillService _billService;
    private readonly ILogger<BillsController> _logger;

    public BillsController(IBillService billService, ILogger<BillsController> logger)
    {
        _billService = billService;
        _logger = logger;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<BillDto>>> GetBillById(long id)
    {
        _logger.LogInformation("GET /api/v1/bills/{BillId} - UserId: {UserId}", id, UserId);

        var billDto = await _billService.GetByIdAsync(id, UserId);
        if (billDto == null)
        {
            _logger.LogWarning("Bill not found. BillId: {BillId}, UserId: {UserId}", id, UserId);
            return NotFound(ApiResponse<string>.ErrorResponse("Bill not found"));
        }

        return Ok(ApiResponse<BillDto>.SuccessResponse(billDto));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BillDto>>>> GetAllBills()
    {
        _logger.LogInformation("GET /api/v1/bills - UserId: {UserId}", UserId);

        var billDtos = await _billService.GetAllAsync(UserId);
        return Ok(ApiResponse<IEnumerable<BillDto>>.SuccessResponse(billDtos));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BillDto>>> CreateBill([FromBody] BillCreateDto dto)
    {
        _logger.LogInformation("POST /api/v1/bills - UserId: {UserId}", UserId);

        var billDto = await _billService.CreateAsync(dto, UserId);
        return CreatedAtAction(nameof(GetBillById), new { id = billDto.Id },
            ApiResponse<BillDto>.SuccessResponse(billDto, "Bill created successfully"));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<BillDto>>> UpdateBill(long id, [FromBody] BillUpdateDto dto)
    {
        _logger.LogInformation("PUT /api/v1/bills/{BillId} - UserId: {UserId}", id, UserId);

        var billDto = await _billService.UpdateAsync(id, dto, UserId);
        if (billDto == null)
        {
            _logger.LogWarning("Update failed. Bill not found. BillId: {BillId}, UserId: {UserId}", id, UserId);
            return NotFound(ApiResponse<string>.ErrorResponse("Bill not found"));
        }

        return Ok(ApiResponse<BillDto>.SuccessResponse(billDto, "Bill updated successfully"));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteBill(long id)
    {
        _logger.LogInformation("DELETE /api/v1/bills/{BillId} - UserId: {UserId}", id, UserId);

        var isDeleted = await _billService.DeleteAsync(id, UserId);
        if (!isDeleted)
        {
            _logger.LogWarning("Delete failed. Bill not found. BillId: {BillId}, UserId: {UserId}", id, UserId);
        }

        return isDeleted
            ? Ok(ApiResponse<string>.SuccessResponse("Bill deleted successfully"))
            : NotFound(ApiResponse<string>.ErrorResponse("Bill not found"));
    }

    [HttpPatch("{id}/deactivate")]
    public async Task<ActionResult<ApiResponse<string>>> DeactivateBill(long id)
    {
        _logger.LogInformation("DELETE /api/v1/bills/deactivate/{BillId} - UserId: {UserId}", id, UserId);
        
        var isDeactivated = await _billService.DeactivateAsync(id, UserId);
        if (!isDeactivated)
        {
            _logger.LogWarning("Deactivate failed. Bill not found. BillId: {BillId}, UserId: {UserId}", id, UserId);
        }

        return isDeactivated
            ? Ok(ApiResponse<string>.SuccessResponse("Bill deactivated successfully"))
            : NotFound(ApiResponse<string>.ErrorResponse("Bill not found"));
    }
}