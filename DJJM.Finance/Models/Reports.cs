namespace DJJM.Finance.Reports;

public class BudgetReport
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpenses { get; set; }
    public decimal TotalDebtPayments { get; set; }
    public decimal TotalSavings { get; set; }
    public decimal TotalRemaining => TotalIncome - (TotalExpenses + TotalDebtPayments + TotalSavings);
}

public class MonthlyReport
{
    public decimal Income { get; set; }
    public decimal FixedExpenses { get; set; }
    public decimal VariableExpenses { get; set; }
    public decimal DebtPayments { get; set; }
    public decimal Savings { get; set; }
    public decimal RemainingBalance => Income - (FixedExpenses + VariableExpenses + DebtPayments + Savings);
}