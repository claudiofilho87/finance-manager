using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Interfaces;

public interface IBillRepository
{
    Task<Bill?> GetByIdAsync(long id, long userId);
    Task<Bill?> GetByIdWithRecurrenceTypeAsync(long id, long userId);
    Task<Bill?> GetByIdWithOccurrencesAsync(long id, long userId);
    Task<IEnumerable<Bill>> GetAllByUserIdAsync(long userId);
    Task<IEnumerable<Bill>> GetInfiniteBillsWithRecurrenceTypeAsync();
    Task AddAsync(Bill bill);
    void Remove(Bill bill);
    Task LoadRecurrenceTypeAsync(Bill bill);
    Task SaveChangesAsync();
}
