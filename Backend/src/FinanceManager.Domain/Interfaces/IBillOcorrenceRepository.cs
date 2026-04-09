using FinanceManager.Domain.Entities;

namespace FinanceManager.Domain.Interfaces;

public interface IBillOcorrenceRepository
{
    Task<BillOcorrence?> GetByIdAsync(long id, long userId);
    Task<BillOcorrence?> GetByIdWithBillAsync(long id);
    Task<IEnumerable<BillOcorrence>> GetAllByUserIdAsync(long userId);
    Task<DateOnly?> GetMaxDateByBillIdAsync(long billId);
    Task AddAsync(BillOcorrence billOcorrence);
    Task AddRangeAsync(IEnumerable<BillOcorrence> billOcorrences);
    void Remove(BillOcorrence billOcorrence);
    void RemoveRange(IEnumerable<BillOcorrence> billOcorrences);
    Task LoadBillAsync(BillOcorrence billOcorrence);
    Task UpdateValueCentsForPendingOccurrencesAsync(long billId, long newValueCents, long oldValueCents, DateOnly fromDate);
    Task SaveChangesAsync();
}
