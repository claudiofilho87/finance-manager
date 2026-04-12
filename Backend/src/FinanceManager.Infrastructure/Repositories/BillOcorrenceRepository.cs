using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Interfaces;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class BillOcorrenceRepository : IBillOcorrenceRepository
{
    private readonly AppDbContext _context;

    public BillOcorrenceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<BillOcorrence?> GetByIdAsync(long id, long userId)
    {
        return await _context.BillOcorrences
            .Include(bo => bo.Bill)
            .FirstOrDefaultAsync(bo => bo.Id == id && bo.Bill != null && bo.Bill.UserId == userId);
    }

    public async Task<BillOcorrence?> GetByIdWithBillAsync(long id, long userId)
    {
        return await _context.BillOcorrences
            .Include(bo => bo.Bill)
            .FirstOrDefaultAsync(bo => bo.Id == id && bo.Bill != null && bo.Bill.UserId == userId);
    }

    public async Task<IEnumerable<BillOcorrence>> GetAllByUserIdAsync(long userId)
    {
        return await _context.BillOcorrences
            .Include(bo => bo.Bill)
            .Where(bo => bo.Bill != null && bo.Bill.UserId == userId)
            .ToListAsync();
    }

    public async Task<DateOnly?> GetMaxDateByBillIdAsync(long billId)
    {
        var maxDate = await _context.BillOcorrences
            .Where(bo => bo.BillId == billId)
            .MaxAsync(bo => (DateOnly?)bo.Date);
        return maxDate;
    }

    public async Task AddAsync(BillOcorrence billOcorrence)
    {
        await _context.BillOcorrences.AddAsync(billOcorrence);
    }

    public async Task AddRangeAsync(IEnumerable<BillOcorrence> billOcorrences)
    {
        await _context.BillOcorrences.AddRangeAsync(billOcorrences);
    }

    public void Remove(BillOcorrence billOcorrence)
    {
        _context.BillOcorrences.Remove(billOcorrence);
    }

    public void RemoveRange(IEnumerable<BillOcorrence> billOcorrences)
    {
        _context.BillOcorrences.RemoveRange(billOcorrences);
    }

    public async Task LoadBillAsync(BillOcorrence billOcorrence)
    {
        await _context.Entry(billOcorrence)
            .Reference(bo => bo.Bill)
            .LoadAsync();
    }

    public async Task UpdateValueCentsForPendingOccurrencesAsync(
        long billId, long newValueCents, long oldValueCents, DateOnly fromDate)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync($@"
            UPDATE bill_ocorrences
            SET value_cents = {newValueCents}
            WHERE bill_id = {billId}
              AND value_cents = {oldValueCents}
              AND date >= {fromDate};
        ");
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
