using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces;
using FinanceManager.Application.Mappings;
using FinanceManager.Domain.Interfaces;

namespace FinanceManager.Application.Services;

public class RecurrenceTypeService : IRecurrenceTypeService
{
    private readonly IRecurrenceTypeRepository _recurrenceTypeRepository;

    public RecurrenceTypeService(IRecurrenceTypeRepository recurrenceTypeRepository)
    {
        _recurrenceTypeRepository = recurrenceTypeRepository;
    }

    public async Task<RecurrenceTypeDto?> GetByIdAsync(long id)
    {
        var recurrenceType = await _recurrenceTypeRepository.GetByIdAsync(id);
        if (recurrenceType == null) return null;
        return RecurrenceTypeMappings.ToDto.Compile()(recurrenceType);
    }

    public async Task<IEnumerable<RecurrenceTypeDto>> GetAllAsync()
    {
        var recurrenceTypes = await _recurrenceTypeRepository.GetAllAsync();
        return recurrenceTypes.Select(RecurrenceTypeMappings.ToDto.Compile());
    }

}
