using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Domain.Entities;

public class Bill
{
    public long Id { get; set; }

    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public long ValueCents { get; set; }

    public DateOnly StartDate { get; set; }

    public long RecurrenceTypeId { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }

    public int? RepeatCount { get; set; }

    public long UserId { get; set; }
    public User? User { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<BillOcorrence> BillOcorrences { get; set; } = new List<BillOcorrence>();
}
