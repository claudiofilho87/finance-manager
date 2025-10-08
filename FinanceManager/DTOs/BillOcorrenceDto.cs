using FinanceManager.Models.Enums;

namespace FinanceManager.DTOs;

public class BillOcorrenceDto
{
    public long Id { get; set; }
    public string Bill { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public BillStatus Status { get; set; }
}