namespace DJJM.Finance.Income;

public class IncomeSource
{
    public string Name { get; set; } = null!;
    public decimal MonthlyAmount { get; set; }
    public IncomeType Type { get; set; }  // Salary, Rental, Commission, etc.
}

public enum IncomeType
{
    Salary,
    Rental,
    Commission,
    Other
}

public class TotalIncome
{
    public List<IncomeSource> Sources { get; set; } = new List<IncomeSource>();
    public decimal CalculateTotal() => Sources.Sum(source => source.MonthlyAmount);
}