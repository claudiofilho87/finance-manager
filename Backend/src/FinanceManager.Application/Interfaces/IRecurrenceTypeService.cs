using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces;

public interface IRecurrenceTypeService
{
    Task<RecurrenceTypeDto?> GetByIdAsync(long id);
    Task<IEnumerable<RecurrenceTypeDto>> GetAllAsync();
}
