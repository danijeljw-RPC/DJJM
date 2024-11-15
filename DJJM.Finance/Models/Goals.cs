namespace DJJM.Finance.Goals;

public class FinancialGoal
{
    public string Description { get; set; } = null!;
    public decimal TargetAmount { get; set; }
    public decimal CurrentSavings { get; set; }

    public decimal RemainingAmount() => TargetAmount - CurrentSavings;
    public bool IsGoalAchieved => CurrentSavings >= TargetAmount;
}

public class GoalManager
{
    public List<FinancialGoal> Goals { get; set; } = new();

    public decimal TotalGoalAmount() => Goals.Sum(goal => goal.TargetAmount);
    public decimal TotalSavedAmount() => Goals.Sum(goal => goal.CurrentSavings);
}