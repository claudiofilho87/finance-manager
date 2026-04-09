using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Domain.Entities;

public class RecurrenceType
{
    public long Id { get; set; }

    [StringLength(255)]
    public string Type { get; set; } = string.Empty;

    public int DaysInterval { get; set; }

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
