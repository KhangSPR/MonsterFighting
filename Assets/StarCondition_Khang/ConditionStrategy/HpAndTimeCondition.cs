[System.Serializable]
public class HpAndTimeCondition : ILevelCondition
{
    public float RequiredHpPercentage { get; set; }
    public float RequiredCompletionTime { get; set; }
    public bool IsConditionMet(float hpPercentage, float completionTime)
    {
        return hpPercentage >= RequiredHpPercentage &&
               completionTime <= RequiredCompletionTime;
    }

    protected override bool IsConditionMet()
    {
        return false;
    }
}
