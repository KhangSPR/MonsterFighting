[System.Serializable]
public class EnemyKillCondition : ILevelCondition
{
    public int RequiredEnemyKills { get; set; }


    public bool IsConditionMet(int enemyKills)
    {
        return enemyKills >= RequiredEnemyKills;
    }
    protected override bool IsConditionMet()
    {
        // Ở đây bạn cần có cách để lấy số kẻ thù đã tiêu diệt
        // Ví dụ: return IsConditionMet(GetEnemyKills());
        return false;
    }
}