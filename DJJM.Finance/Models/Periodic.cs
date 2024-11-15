namespace DJJM.Finance.Periodic;

public class PeriodicExpense
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public ExpenseType Type { get; set; }  // Sinking Fund, Annual Fee, etc.
}

public enum ExpenseType
{
    SinkingFund,
    AnnualFee,
    HolidayExpense,
    Other
}