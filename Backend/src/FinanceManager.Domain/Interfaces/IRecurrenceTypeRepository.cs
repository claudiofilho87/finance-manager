using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Interfaces;

public interface IRecurrenceTypeRepository
{
    Task<RecurrenceType?> GetByIdAsync(long id);
    Task<IEnumerable<RecurrenceType>> GetAllAsync();
}
