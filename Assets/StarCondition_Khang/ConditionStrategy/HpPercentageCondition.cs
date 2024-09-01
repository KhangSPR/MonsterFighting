using UnityEngine;

[System.Serializable]
public class HpPercentageCondition : ILevelCondition
{
    public float RequiredHpPercentage { get; set; }


    public bool IsConditionMet(float currentHpPercentage)
    {
        return currentHpPercentage >= RequiredHpPercentage;
    }

    // Triển khai phương thức kiểm tra không tham số
    protected override bool IsConditionMet()
    {
        return false;
    }
}