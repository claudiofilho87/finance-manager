using FinanceManager.Models.Enums;

namespace FinanceManager.DTOs;

public class BillOcorrenceCreateDto
{
    public long BillId { get; set; }
    public DateOnly Date { get; set; }
    public BillStatus Status { get; set; }
}