using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.DTOs;

public class BillOcorrenceCreateDto
{
    public long BillId { get; set; }
    public DateOnly Date { get; set; }
    public BillStatus Status { get; set; }
    public decimal? Value { get; set; }
    public string? Observation { get; set; }
}
