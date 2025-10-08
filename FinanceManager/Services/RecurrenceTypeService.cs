using FinanceManager.Data;
using FinanceManager.DTOs;
using FinanceManager.Expressions;
using FinanceManager.Models;
using FinanceManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Services;

public class RecurrenceTypeService : IRecurrenceTypeService
{
    private readonly AppDbContext _context;

    public RecurrenceTypeService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<RecurrenceTypeDto?> GetByIdAsync(long id)
    {
        return await _context.RecurrenceTypes
            .Where(recurrence => recurrence.Id == id)
            .Select(RecurrenceTypeExpressions.ToDto)
            .FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<RecurrenceTypeDto>> GetAllAsync()
    {
        return await _context.RecurrenceTypes
            .Select(RecurrenceTypeExpressions.ToDto)
            .ToListAsync();
    }

    public async Task<RecurrenceTypeDto> CreateAsync(RecurrenceTypeCreateDto dto)
    {
        var now = DateTime.UtcNow;
        var recurrence = new RecurrenceType
        {
            Type = dto.Type,
            DaysInterval = dto.DaysInterval
        };

        await _context.RecurrenceTypes.AddAsync(recurrence);
        await _context.SaveChangesAsync();

        return RecurrenceTypeExpressions.ToDto.Compile()(recurrence);
    }

    public async Task<RecurrenceTypeDto?> UpdateAsync(long id, RecurrenceTypeCreateDto dto)
    {
        var recurrence = await _context.RecurrenceTypes.FindAsync(id);

        if (recurrence == null) return null;

        recurrence.Type = dto.Type;
        recurrence.DaysInterval = dto.DaysInterval;
        await _context.SaveChangesAsync();
        
        return RecurrenceTypeExpressions.ToDto.Compile()(recurrence);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var recurrence = await _context.RecurrenceTypes.FindAsync(id);
        if (recurrence == null) return false;
        
        _context.RecurrenceTypes.Remove(recurrence);
        await _context.SaveChangesAsync();

        return true;
    }
}