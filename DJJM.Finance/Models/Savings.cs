namespace DJJM.Finance.Savings;
public class SavingsGoal
{
    public string GoalName { get; set; } = null!;
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }

    public decimal RemainingAmount() => TargetAmount - CurrentAmount;
    public bool IsGoalReached => CurrentAmount >= TargetAmount;
}

public class EmergencyFund
{
    public decimal TargetAmount { get; set; }
    public decimal CurrentAmount { get; set; }
}
