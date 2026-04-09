using FinanceManager.Application.DTOs;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Interfaces;

public interface IBillOcorrenceService
{
    Task<BillOcorrenceDto?> GetByIdAsync(long id, long userId);
    Task<IEnumerable<BillOcorrenceDto>> GetAllAsync(long userId);
    Task<BillOcorrenceDto?> CreateAsync(BillOcorrenceCreateDto dto, long userId);
    Task<BillOcorrenceDto?> UpdateAsync(long id, BillOcorrenceCreateDto dto, long userId);
    Task<bool> DeleteAsync(long id, long userId);
    Task CreateMultiplesAsync(Bill bill);
    Task UpdateMultiplesAsync(Bill bill, long oldValueCents);
    Task DeleteMultiplesAsync(Bill bill);
}
