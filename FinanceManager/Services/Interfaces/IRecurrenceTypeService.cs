using FinanceManager.DTOs;

namespace FinanceManager.Services.Interfaces;

public interface IRecurrenceTypeService
{
    public Task<RecurrenceTypeDto?> GetByIdAsync(long id);
    public Task<IEnumerable<RecurrenceTypeDto>> GetAllAsync();
    public Task<RecurrenceTypeDto> CreateAsync(RecurrenceTypeCreateDto dto);
    public Task<RecurrenceTypeDto?> UpdateAsync(long id, RecurrenceTypeCreateDto dto);
    public Task<bool> DeleteAsync(long id);
}