using FinanceManager.DTOs;
using FinanceManager.Models;

namespace FinanceManager.Services.Interfaces;

public interface IBillOcorrenceService
{
    public Task<BillOcorrenceDto?> GetByIdAsync(long id, long userId);
    public Task<IEnumerable<BillOcorrenceDto>> GetAllAsync(long userId);
    public Task<BillOcorrenceDto?> CreateAsync(BillOcorrenceCreateDto dto, long userId);
    public Task<BillOcorrenceDto?> UpdateAsync(long id, BillOcorrenceCreateDto dto, long userId);
    public Task<bool> DeleteAsync(long id, long userId);
    public Task CreateMultiplesAsync(Bill bill);
}