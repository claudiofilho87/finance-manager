using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces;

public interface IBillService
{
    Task<BillDto?> GetByIdAsync(long id, long userId);
    Task<IEnumerable<BillDto>> GetAllAsync(long userId);
    Task<BillDto> CreateAsync(BillCreateDto dto, long userId);
    Task<BillDto?> UpdateAsync(long id, BillUpdateDto dto, long userId);
    Task<bool> DeleteAsync(long id, long userId);
    Task<bool> DeactivateAsync(long id, long userId);
}
