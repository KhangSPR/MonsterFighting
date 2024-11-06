using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildMoreDefault", menuName = "UIGuild/MoreAbility", order = 4)]
public class GuildMoreStatsAbilitySO : GuildDefaultStatsSO
{
    [Header("Character Class")]
    public AttackCategory characterClass; // Class của nhân vật

    public override void ApplyMoreStats(GameObject character)
    {
        // Chọn chiến lược phù hợp dựa trên class của nhân vật
        _IStatIncreaseStrategy statIncreaseStrategy = GetStatIncreaseStrategy();

        // Áp dụng chiến lược tăng chỉ số
        statIncreaseStrategy?.IncreaseStats(character, characterClass);
    }

    private _IStatIncreaseStrategy GetStatIncreaseStrategy()
    {
        // Chọn chiến lược tăng chỉ số dựa trên characterClass
        switch (characterClass)
        {
            case AttackCategory.Warrior:
                return new WarriorStatIncreaseStrategy();
            case AttackCategory.Archer:
                return new ArcherStatIncreaseStrategy();
            case AttackCategory.Wizard:
                return new WizardStatIncreaseStrategy();
            default:
                Debug.LogWarning("Class không được hỗ trợ!");
                return null;
        }
    }
}
