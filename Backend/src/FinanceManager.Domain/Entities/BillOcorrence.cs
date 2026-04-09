using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class BillOcorrence
{
    public long Id { get; set; }

    public long BillId { get; set; }
    public Bill? Bill { get; set; }

    public DateOnly Date { get; set; }

    public BillStatus Status { get; set; }

    public long ValueCents { get; set; }

    public string? Observation { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
