using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Application.Mappings;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Factories;
using FinanceManager.Domain.Interfaces;

namespace FinanceManager.Application.Services;

public class BillService : IBillService
{
    private readonly IBillRepository _billRepository;
    private readonly IBillOcorrenceService _billOcorrenceService;
    private readonly IBillOcorrenceFactory _billOcorrenceFactory;
    private readonly IUnitOfWork _unitOfWork;

    public BillService(
        IBillRepository billRepository,
        IBillOcorrenceService billOcorrenceService,
        IBillOcorrenceFactory billOcorrenceFactory,
        IUnitOfWork unitOfWork)
    {
        _billRepository = billRepository;
        _billOcorrenceService = billOcorrenceService;
        _billOcorrenceFactory = billOcorrenceFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<BillDto?> GetByIdAsync(long id, long userId)
    {
        var bill = await _billRepository.GetByIdAsync(id, userId);
        if (bill == null) return null;
        return BillMappings.ToDto.Compile()(bill);
    }

    public async Task<IEnumerable<BillDto>> GetAllAsync(long userId)
    {
        var bills = await _billRepository.GetAllByUserIdAsync(userId);
        return bills.Select(BillMappings.ToDto.Compile());
    }

    public async Task<BillDto> CreateAsync(BillCreateDto dto, long userId)
    {
        var now = DateTime.UtcNow;
        var bill = new Bill
        {
            Name = dto.Name,
            Description = dto.Description,
            ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0,
            StartDate = dto.StartDate,
            RecurrenceTypeId = dto.RecurrenceTypeId,
            RepeatCount = dto.RepeatCount.HasValue ? dto.RepeatCount : null,
            UserId = userId,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _billRepository.AddAsync(bill);
        await _unitOfWork.CommitAsync();

        await _billRepository.LoadRecurrenceTypeAsync(bill);
        await _billOcorrenceService.CreateMultiplesAsync(bill);
        await _unitOfWork.CommitAsync();

        return BillMappings.ToDto.Compile()(bill);
    }

    public async Task<BillDto?> UpdateAsync(long id, BillUpdateDto dto, long userId)
    {
        var bill = await _billRepository.GetByIdWithRecurrenceTypeAsync(id, userId);
        if (bill == null) return null;

        var oldValueCents = bill.ValueCents;
        bill.Name = dto.Name;
        bill.Description = dto.Description;
        bill.ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0;
        bill.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.CommitAsync();
        await _billOcorrenceService.UpdateMultiplesAsync(bill, oldValueCents);

        return BillMappings.ToDto.Compile()(bill);
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        var bill = await _billRepository.GetByIdAsync(id, userId);
        if (bill == null) return false;

        _billRepository.Remove(bill);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public async Task<bool> DeactivateAsync(long id, long userId)
    {
        var bill = await _billRepository.GetByIdWithOccurrencesAsync(id, userId);
        if (bill == null) return false;

        bill.IsActive = false;
        await _billOcorrenceService.DeleteMultiplesAsync(bill);
        await _unitOfWork.CommitAsync();

        return true;
    }
}
