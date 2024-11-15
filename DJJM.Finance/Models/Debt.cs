namespace DJJM.Finance.Debt;

public class DebtAccount
{
    public string Name { get; set; } = null!;
    public decimal Balance { get; set; }
    public decimal InterestRate { get; set; }
    public decimal MonthlyPayment { get; set; }
}

public class DebtManager
{
    public List<DebtAccount> Debts { get; set; } = new();

    public decimal CalculateTotalDebt() => Debts.Sum(debt => debt.Balance);
    public decimal CalculateMonthlyPayments() => Debts.Sum(debt => debt.MonthlyPayment);
}