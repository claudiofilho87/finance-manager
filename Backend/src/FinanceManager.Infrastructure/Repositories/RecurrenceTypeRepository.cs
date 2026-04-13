using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Interfaces;
using FinanceManager.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Infrastructure.Repositories;

public class RecurrenceTypeRepository : IRecurrenceTypeRepository
{
    private readonly AppDbContext _context;

    public RecurrenceTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RecurrenceType?> GetByIdAsync(long id)
    {
        return await _context.RecurrenceTypes.FindAsync(id);
    }

    public async Task<IEnumerable<RecurrenceType>> GetAllAsync()
    {
        return await _context.RecurrenceTypes.ToListAsync();
    }

    public async Task AddAsync(RecurrenceType recurrenceType)
    {
        await _context.RecurrenceTypes.AddAsync(recurrenceType);
    }

    public void Remove(RecurrenceType recurrenceType)
    {
        _context.RecurrenceTypes.Remove(recurrenceType);
    }

}
