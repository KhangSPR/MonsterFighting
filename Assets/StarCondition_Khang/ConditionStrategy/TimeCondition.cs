[System.Serializable]
public class TimeCondition : ILevelCondition
{
    public float RequiredTime { get; set; }

    public bool IsConditionMet(float completionTime)
    {
        return completionTime <= RequiredTime;
    }

    protected override bool IsConditionMet()
    {
        // Ở đây bạn cần có cách để lấy thời gian hoàn thành
        // Ví dụ: return IsConditionMet(GetCompletionTime());
        return false;
    }
}