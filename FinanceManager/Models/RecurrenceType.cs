using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

[Table("recurrence_types")]
public class RecurrenceType
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("type")]
    [StringLength(255)]
    public string Type { get; set; } = string.Empty;
    
    [Column("days_interval")]
    public int DaysInterval { get; set; }

    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
}