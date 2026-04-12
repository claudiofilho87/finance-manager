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

    public BillOcorrenceService(
        IBillOcorrenceRepository billOcorrenceRepository,
        IBillRepository billRepository,
        IBillOcorrenceFactory billOcorrenceFactory)
    {
        _billOcorrenceRepository = billOcorrenceRepository;
        _billRepository = billRepository;
        _billOcorrenceFactory = billOcorrenceFactory;
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
        await _billOcorrenceRepository.SaveChangesAsync();
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

        await _billOcorrenceRepository.SaveChangesAsync();

        return BillOcorrenceMappings.ToDto.Compile()(billOcorrence);
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        var billOcorrence = await _billOcorrenceRepository.GetByIdAsync(id, userId);
        if (billOcorrence == null) return false;

        _billOcorrenceRepository.Remove(billOcorrence);
        await _billOcorrenceRepository.SaveChangesAsync();

        return true;
    }

    public async Task CreateMultiplesAsync(Bill bill)
    {
        var occurrences = _billOcorrenceFactory.Generate(bill).ToList();
        if (!occurrences.Any()) return;

        await _billOcorrenceRepository.AddRangeAsync(occurrences);
        await _billOcorrenceRepository.SaveChangesAsync();
    }

    public async Task UpdateMultiplesAsync(Bill bill, long oldValueCents)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        await _billOcorrenceRepository.UpdateValueCentsForPendingOccurrencesAsync(
            bill.Id, bill.ValueCents, oldValueCents, today);
    }

    public async Task DeleteMultiplesAsync(Bill bill)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var occurrences = bill.BillOcorrences
            .Where(bo => bo.Date >= today)
            .ToList();

        if (!occurrences.Any()) return;

        _billOcorrenceRepository.RemoveRange(occurrences);
        await _billOcorrenceRepository.SaveChangesAsync();
    }
}
