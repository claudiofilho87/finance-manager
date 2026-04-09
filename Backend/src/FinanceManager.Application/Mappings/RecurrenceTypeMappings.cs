using System.Linq.Expressions;
using FinanceManager.Application.DTOs;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Mappings;

public class RecurrenceTypeMappings
{
    public static Expression<Func<RecurrenceType, RecurrenceTypeDto>> ToDto = recurrence => new RecurrenceTypeDto
    {
        Id = recurrence.Id,
        Type = recurrence.Type,
        DaysInterval = recurrence.DaysInterval
    };
}
