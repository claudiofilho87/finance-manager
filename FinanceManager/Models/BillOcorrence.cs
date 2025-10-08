using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Models.Enums;

namespace FinanceManager.Models;

[Table("bill_ocorrences")]
public class BillOcorrence
{
    [Column("id")]
    public long Id { get; set; }
    
    [ForeignKey("Bill")]
    [Column("bill_id")]
    public long BillId { get; set; }
    public Bill? Bill { get; set; }
    
    [Column("date")]
    public DateOnly Date { get; set; }
    
    [Column("status")]
    public BillStatus Status { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}