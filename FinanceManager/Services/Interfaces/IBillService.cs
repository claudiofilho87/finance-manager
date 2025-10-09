using FinanceManager.DTOs;

namespace FinanceManager.Services.Interfaces;

public interface IBillService
{
    public Task<BillDto?> GetByIdAsync(long id, long userId);
    public Task<IEnumerable<BillDto>> GetAllAsync(long userId);
    public Task<BillDto> CreateAsync(BillCreateDto dto, long userId);
    public Task<BillDto?> UpdateAsync(long id, BillUpdateDto dto, long userId);
    public Task<bool> DeleteAsync(long id, long userId);
    public Task<bool> DeactivateAsync(long id, long userId);
}