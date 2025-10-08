using FinanceManager.Data;
using FinanceManager.DTOs;
using FinanceManager.Expressions;
using FinanceManager.Models;
using FinanceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services;

public class BillService : IBillService
{
    private readonly AppDbContext _context;
    private readonly IBillOcorrenceService _billOcorrenceService;
    
    public BillService(AppDbContext context, IBillOcorrenceService billOcorrenceService)
    {
        _context = context;
        _billOcorrenceService = billOcorrenceService;
    }
    
    public async Task<BillDto?> GetByIdAsync(long id, long userId)
    {
        return await _context.Bills
            .Where(bill => bill.Id == id && bill.UserId == userId)
            .Select(BillExpressions.ToDto)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<BillDto>> GetAllAsync(long userId)
    {
        return await _context.Bills
            .Where(bill => bill.UserId == userId)
            .Select(BillExpressions.ToDto)
            .ToListAsync();
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

        await _context.Bills.AddAsync(bill);
        await _context.SaveChangesAsync();
        await _context.Entry(bill)
            .Reference(newBill => newBill.RecurrenceType)
            .LoadAsync();
        await _billOcorrenceService.CreateMultiplesAsync(bill);
        
        return BillExpressions.ToDto.Compile()(bill);
    }

    public async Task<BillDto?> UpdateAsync(long id, BillCreateDto dto, long userId)
    {
        var bill = await _context.Bills
            .Include(bill => bill.RecurrenceType)
            .FirstOrDefaultAsync(bill => bill.Id == id);

        if (bill == null || bill.UserId != userId) return null;

        bill.Name = dto.Name;
        bill.Description = dto.Description;
        bill.ValueCents = dto.Value.HasValue ? (long)(dto.Value * 100) : 0;
        bill.StartDate = dto.StartDate;
        bill.RecurrenceTypeId = dto.RecurrenceTypeId;
        bill.RepeatCount = dto.RepeatCount;
        bill.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return BillExpressions.ToDto.Compile()(bill);
    }

    public async Task<bool> DeleteAsync(long id, long userId)
    {
        var bill = await _context.Bills.FindAsync(id);
        if (bill == null || bill.UserId != userId) return false;

        _context.Bills.Remove(bill);
        await _context.SaveChangesAsync();

        return true;
    }
}