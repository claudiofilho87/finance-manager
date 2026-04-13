using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Application.Mappings;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Factories;
using FinanceManager.Domain.Interfaces;

namespace FinanceManager.Application.Services;

public class BillOcorrenceService : IBillOcorrenceService
{
    private readonly IBillOcorrenceRepository _billOcorrenceRepository;
    private readonly IBillRepository _billRepository;
    private readonly IBillOcorrenceFactory _billOcorrenceFactory;
    private readonly IUnitOfWork _unitOfWork;

    public BillOcorrenceService(
        IBillOcorrenceRepository billOcorrenceRepository,
        IBillRepository billRepository,
        IBillOcorrenceFactory billOcorrenceFactory,
        IUnitOfWork unitOfWork)
    {
        _billOcorrenceRepository = billOcorrenceRepository;
        _billRepository = billRepository;
        _billOcorrenceFactory = billOcorrenceFactory;
        _unitOfWork = unitOfWork;
    }

    public async Task<BillOcorrenceDto?> GetByIdAsync(long id, long userId)
    {
        var billOcorrence = await _billOcorrenceRepository.GetByIdAsync(id, userId);
        if (billOcorrence == null) return null;
        return BillOcorrenceMappings.ToDto.Compile()(billOcorrence);
    }

    public async Task<IEnumerable<BillOcorrenceDto>> GetAllAsync(long userId)
    {
        var occurrences = await _billOcorrenceRepository.GetAllByUserIdAsync(userId);
        return occurrences.Select(BillOcorrenceMappings.ToDto.Compile());
    }

    public async Task<BillOcorrenceDto?> CreateAsync(BillOcorrenceCreateDto dto, long userId)
    {
        var bill = await _billRepository.GetByIdAsync(dto.BillId, userId);
        if (bill == null) return null;

        var now = DateTime.UtcNow;
        var billOcorrence = new BillOcorrence
        {
            BillId = dto.BillId,
            Date = dto.Date,
            Status = dto.Status,
            ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0,
            Observation = dto.Observation,
            CreatedAt = now,
            UpdatedAt = now
        };

        await _billOcorrenceRepository.AddAsync(billOcorrence);
        await _unitOfWork.CommitAsync();
        await _billOcorrenceRepository.LoadBillAsync(billOcorrence);

        return BillOcorrenceMappings.ToDto.Compile()(billOcorrence);
    }

    public async Task<BillOcorrenceDto?> UpdateAsync(long id, BillOcorrenceCreateDto dto, long userId)
    {
        var bill = await _billRepository.GetByIdAsync(dto.BillId, userId);
        if (bill == null) return null;

        var billOcorrence = await _billOcorrenceRepository.GetByIdWithBillAsync(id, userId);
        if (billOcorrence == null) return null;

        billOcorrence.BillId = dto.BillId;
        billOcorrence.Date = dto.Date;
        billOcorrence.Status = dto.Status;
        billOcorrence.ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0;
        billOcorrence.Observation = dto.Observation;

        await _unitOfWork.CommitAsync();

        return BillOcorrenceMappings.ToDto.Compile()(billOcorrence);
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        var billOcorrence = await _billOcorrenceRepository.GetByIdAsync(id, userId);
        if (billOcorrence == null) return false;

        _billOcorrenceRepository.Remove(billOcorrence);
        await _unitOfWork.CommitAsync();

        return true;
    }

    public Task CreateMultiplesAsync(Bill bill)
    {
        var occurrences = _billOcorrenceFactory.Generate(bill).ToList();
        if (!occurrences.Any()) return Task.CompletedTask;

        return _billOcorrenceRepository.AddRangeAsync(occurrences);
    }

    public async Task UpdateMultiplesAsync(Bill bill, long oldValueCents)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await _billOcorrenceRepository.UpdateValueCentsForPendingOccurrencesAsync(
            bill.Id, bill.ValueCents, oldValueCents, today);
    }

    public Task DeleteMultiplesAsync(Bill bill)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var occurrences = bill.BillOcorrences
            .Where(bo => bo.Date >= today)
            .ToList();

        if (!occurrences.Any()) return Task.CompletedTask;

        _billOcorrenceRepository.RemoveRange(occurrences);
        return Task.CompletedTask;
    }
}
