using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Interfaces;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class BillRepository : IBillRepository
{
    private readonly AppDbContext _context;

    public BillRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Bill?> GetByIdAsync(long id, long userId)
    {
        return await _context.Bills
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<Bill?> GetByIdWithRecurrenceTypeAsync(long id, long userId)
    {
        return await _context.Bills
            .Include(b => b.RecurrenceType)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<Bill?> GetByIdWithOccurrencesAsync(long id, long userId)
    {
        return await _context.Bills
            .Include(b => b.BillOcorrences)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
    }

    public async Task<IEnumerable<Bill>> GetAllByUserIdAsync(long userId)
    {
        return await _context.Bills
            .Where(b => b.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Bill>> GetInfiniteBillsWithRecurrenceTypeAsync()
    {
        return await _context.Bills
            .Include(b => b.BillOcorrences)
            .Include(b => b.RecurrenceType)
            .Where(b => b.RepeatCount == null)
            .ToListAsync();
    }

    public async Task AddAsync(Bill bill)
    {
        await _context.Bills.AddAsync(bill);
    }

    public void Remove(Bill bill)
    {
        _context.Bills.Remove(bill);
    }

    public async Task LoadRecurrenceTypeAsync(Bill bill)
    {
        await _context.Entry(bill)
            .Reference(b => b.RecurrenceType)
            .LoadAsync();
    }

}
