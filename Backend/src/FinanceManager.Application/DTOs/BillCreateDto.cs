namespace FinanceManager.Application.DTOs;

public class BillCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Value { get; set; }
    public DateOnly StartDate { get; set; }
    public long RecurrenceTypeId { get; set; }
    public int? RepeatCount { get; set; }
}
