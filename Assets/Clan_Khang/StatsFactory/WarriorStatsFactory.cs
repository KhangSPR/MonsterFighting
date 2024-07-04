using UnityEngine;
using UIGameDataManager;

public class WarriorStatsFactory : _ICharacterStatsFactory
{
    public Stats CreateCharacterStats(int attack, int life, float attackSpeed, float specialAttack)
    {
        // Create a new CardCharacter instance with the basic stats
        return new Stats(attack, life, attackSpeed, specialAttack);
    }

    public _IStatIncreaseStrategy CreateStatIncreaseStrategy()
    {
        // Return a new instance of the WarriorStatIncreaseStrategy
        return new WarriorStatIncreaseStrategy();
    }
}
