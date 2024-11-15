namespace DJJM.Finance.Expenses;

public class FixedExpense
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
    public Frequency Frequency { get; set; }  // Monthly, Annual, etc.
}

public class VariableExpense
{
    public string Name { get; set; } = null!;
    public decimal Amount { get; set; }
}

public class ExpenseSummary
{
    public List<FixedExpense> FixedExpenses { get; set; } = new();
    public List<VariableExpense> VariableExpenses { get; set; } = new();

    public decimal CalculateTotalFixed() => FixedExpenses.Sum(expense => expense.Amount);
    public decimal CalculateTotalVariable() => VariableExpenses.Sum(expense => expense.Amount);
}

public enum Frequency
{
    Daily,
    Weekly,
    Fortnightly,
    BiMonthly,
    Monthly,
    Quarterly,
    Annual
}