namespace FinanceManager.DTOs;

public class RecurrenceTypeDto
{
    public long Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int DaysInterval { get; set; }
}