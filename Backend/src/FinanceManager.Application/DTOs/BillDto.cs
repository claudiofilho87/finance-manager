namespace FinanceManager.Application.DTOs;

public class BillDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Value { get; set; }
    public DateOnly StartDate { get; set; }
    public long RecurrenceTypeId { get; set; }
    public int? RepeatCount { get; set; }
    public bool IsActive { get; set; } = true;
}
