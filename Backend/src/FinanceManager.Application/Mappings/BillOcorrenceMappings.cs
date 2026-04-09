using System.Linq.Expressions;
using FinanceManager.Application.DTOs;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Mappings;

public class BillOcorrenceMappings
{
    public static Expression<Func<BillOcorrence, BillOcorrenceDto>> ToDto = billOcorrence => new BillOcorrenceDto
    {
        Id = billOcorrence.Id,
        BillId = billOcorrence.BillId,
        Bill = billOcorrence.Bill != null ? billOcorrence.Bill.Name : string.Empty,
        Date = billOcorrence.Date,
        Status = billOcorrence.Status,
        Value = Math.Round(billOcorrence.ValueCents / 100m, 2),
        Observation = billOcorrence.Observation
    };
}
