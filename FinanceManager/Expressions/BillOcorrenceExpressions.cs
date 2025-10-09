using System.Linq.Expressions;
using FinanceManager.DTOs;
using FinanceManager.Models;
using FinanceManager.Models.Enums;

namespace FinanceManager.Expressions;

public class BillOcorrenceExpressions
{
    public static Expression<Func<BillOcorrence, BillOcorrenceDto>> ToDto = billOcorrence => new BillOcorrenceDto
    {
        Id = billOcorrence.Id,
        Bill = billOcorrence.Bill != null ?  billOcorrence.Bill.Name : string.Empty,
        Date = billOcorrence.Date,
        Status = billOcorrence.Status,
        Value = Math.Round(billOcorrence.ValueCents / 100m, 2),
        Observation = billOcorrence.Observation
    };
}