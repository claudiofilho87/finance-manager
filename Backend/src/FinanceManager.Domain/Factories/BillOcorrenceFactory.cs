using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Factories;

public class BillOcorrenceFactory : IBillOcorrenceFactory
{
    public IEnumerable<BillOcorrence> Generate(Bill bill)
    {
        if (bill.RecurrenceType?.DaysInterval == null)
            return Enumerable.Empty<BillOcorrence>();

        var ocorrences = new List<BillOcorrence>();
        var now = DateTime.UtcNow;
        var repeatCount = bill.RepeatCount ?? 6;

        for (int i = 0; i < repeatCount; i++)
        {
            ocorrences.Add(new BillOcorrence
            {
                BillId = bill.Id,
                Date = bill.RecurrenceType.DaysInterval switch
                {
                    (int)RecurrenceTypeEnum.Monthly => bill.StartDate.AddMonths(i),
                    (int)RecurrenceTypeEnum.Quarterly => bill.StartDate.AddMonths(i * 3),
                    (int)RecurrenceTypeEnum.Semiannual => bill.StartDate.AddMonths(i * 6),
                    (int)RecurrenceTypeEnum.Yearly => bill.StartDate.AddYears(i),
                    _ => bill.StartDate.AddDays(i * bill.RecurrenceType.DaysInterval)
                },
                Status = BillStatus.Pending,
                ValueCents = bill.ValueCents,
                CreatedAt = now,
                UpdatedAt = now
            });
        }

        return ocorrences;
    }
}
