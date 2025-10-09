using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Models;

[Table("bills")]
public class Bill
{
    [Column("id")]
    public long Id { get; set; }
    
    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    
    [Column("description")]
    public string? Description { get; set; }
    
    [Column("value_cents")]
    public long ValueCents { get; set; }
    
    [Column("start_date")]
    public DateOnly StartDate { get; set; }
    
    [ForeignKey("RecurrenceType")]
    [Column("recurrence_type_id")]
    public long RecurrenceTypeId { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    
    [Column("repeat_count")]
    public int? RepeatCount { get; set; }
    
    [ForeignKey("User")]
    [Column("user_id")]
    public long UserId { get; set; }
    public User? User { get; set; }

    [Column("is_active")] 
    public bool IsActive { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public ICollection<BillOcorrence> BillOcorrences { get; set; } = new List<BillOcorrence>();
}