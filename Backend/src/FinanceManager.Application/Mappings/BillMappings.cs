using System.Linq.Expressions;
using FinanceManager.Application.DTOs;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Mappings;

public class BillMappings
{
    public static Expression<Func<Bill, BillDto>> ToDto = bill => new BillDto
    {
        Id = bill.Id,
        Name = bill.Name,
        Description = bill.Description,
        Value = Math.Round(bill.ValueCents / 100m, 2),
        StartDate = bill.StartDate,
        RecurrenceTypeId = bill.RecurrenceTypeId,
        RepeatCount = bill.RepeatCount,
        IsActive = bill.IsActive
    };
}
