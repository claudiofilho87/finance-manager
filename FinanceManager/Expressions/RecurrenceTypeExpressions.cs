using System.Linq.Expressions;
using FinanceManager.DTOs;
using FinanceManager.Models;

namespace FinanceManager.Expressions;

public class RecurrenceTypeExpressions
{
    public static Expression<Func<RecurrenceType, RecurrenceTypeDto>> ToDto = recurrence => new RecurrenceTypeDto
    {
        Id = recurrence.Id,
        Type = recurrence.Type,
        DaysInterval = recurrence.DaysInterval
    };
}