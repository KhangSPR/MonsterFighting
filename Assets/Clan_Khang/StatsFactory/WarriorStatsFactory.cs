using UnityEngine;
using UIGameDataManager;

public class WarriorStatsFactory : _ICharacterStatsFactory
{
    public Stats CreateCharacterStats(int attack, int life, int deff, float attackSpeed, float specialAttack, float attackSpeedMelee, float recoveryMana)
    {
        // Create a new CardCharacter instance with the basic stats
        return new Stats(attack, life, deff, attackSpeed, specialAttack, attackSpeedMelee, recoveryMana);
    }

    public _IStatIncreaseStrategy CreateStatIncreaseStrategy()
    {
        // Return a new instance of the WarriorStatIncreaseStrategy
        return new WarriorStatIncreaseStrategy();
    }
}
