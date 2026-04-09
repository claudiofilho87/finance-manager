using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces;

public interface IRecurrenceTypeService
{
    Task<RecurrenceTypeDto?> GetByIdAsync(long id);
    Task<IEnumerable<RecurrenceTypeDto>> GetAllAsync();
    Task<RecurrenceTypeDto> CreateAsync(RecurrenceTypeCreateDto dto);
    Task<RecurrenceTypeDto?> UpdateAsync(long id, RecurrenceTypeCreateDto dto);
    Task<bool> DeleteAsync(long id);
}
