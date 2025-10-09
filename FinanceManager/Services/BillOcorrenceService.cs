using FinanceManager.Data;
using FinanceManager.DTOs;
using FinanceManager.Expressions;
using FinanceManager.Models;
using FinanceManager.Models.Factories;
using FinanceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services;

public class BillOcorrenceService : IBillOcorrenceService
{
    private readonly AppDbContext _context;
    private readonly IBillOcorrenceFactory _billOcorrenceFactory;

    public BillOcorrenceService(AppDbContext context, IBillOcorrenceFactory billOcorrenceFactory)
    {
        _context = context;
        _billOcorrenceFactory = billOcorrenceFactory;
    }

    public async Task<BillOcorrenceDto?> GetByIdAsync(long id, long userId)
    {
        return await _context.BillOcorrences
            .Where(billOcorrence =>
                billOcorrence.Id == id && billOcorrence.Bill != null && billOcorrence.Bill.UserId == userId)
            .Select(BillOcorrenceExpressions.ToDto)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BillOcorrenceDto>> GetAllAsync(long userId)
    {
        return await _context.BillOcorrences
            .Where(billOcorrence => billOcorrence.Bill != null && billOcorrence.Bill.UserId == userId)
            .Select(BillOcorrenceExpressions.ToDto)
            .ToListAsync();
    }

    public async Task<BillOcorrenceDto?> CreateAsync(BillOcorrenceCreateDto dto, long userId)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(bill => bill.Id == dto.BillId && bill.UserId == userId);

        if (bill == null)
            return null;

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

        await _context.BillOcorrences.AddAsync(billOcorrence);
        await _context.SaveChangesAsync();
        await _context.Entry(billOcorrence)
            .Reference(newOcorrence => newOcorrence.Bill)
            .LoadAsync();

        return BillOcorrenceExpressions.ToDto.Compile()(billOcorrence);
    }

    public async Task<BillOcorrenceDto?> UpdateAsync(long id, BillOcorrenceCreateDto dto, long userId)
    {
        var bill = await _context.Bills
            .FirstOrDefaultAsync(bill => bill.Id == dto.BillId && bill.UserId == userId);

        if (bill == null)
            return null;

        var billOcorrence = await _context.BillOcorrences
            .Include(billOcorrence => billOcorrence.Bill)
            .FirstOrDefaultAsync(billOcorrence => billOcorrence.Id == id);

        if (billOcorrence == null) return null;

        billOcorrence.BillId = dto.BillId;
        billOcorrence.Date = dto.Date;
        billOcorrence.Status = dto.Status;
        billOcorrence.ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0;
        billOcorrence.Observation = dto.Observation;

        await _context.SaveChangesAsync();

        return BillOcorrenceExpressions.ToDto.Compile()(billOcorrence);
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        var billOcorrence = await _context.BillOcorrences
            .Include(bo => bo.Bill)
            .FirstOrDefaultAsync(bo => bo.Id == id && bo.Bill != null && bo.Bill.UserId == userId);

        if (billOcorrence == null) return false;

        _context.BillOcorrences.Remove(billOcorrence);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task CreateMultiplesAsync(Bill bill)
    {
        var occurrences = _billOcorrenceFactory.Generate(bill).ToList();
        if (!occurrences.Any()) return;

        await _context.BillOcorrences.AddRangeAsync(occurrences);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateMultiplesAsync(Bill bill, long oldValueCents)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE bill_ocorrences
            SET value_cents = {bill.ValueCents}
            WHERE bill_id = {bill.Id} 
              AND value_cents = {oldValueCents}
              AND date >= {today};
        ");
    }

    public async Task DeleteMultiplesAsync(Bill bill)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        
        var occurrences = bill.BillOcorrences
            .Where(bo => bo.Date >= today)
            .ToList();

        if (!occurrences.Any()) return;

        _context.BillOcorrences.RemoveRange(occurrences);
        await _context.SaveChangesAsync();
    }
}