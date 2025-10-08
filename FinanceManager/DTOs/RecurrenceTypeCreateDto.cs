namespace FinanceManager.DTOs;

public class RecurrenceTypeCreateDto
{
    public string Type { get; set; } = string.Empty;
    public int DaysInterval { get; set; }
}