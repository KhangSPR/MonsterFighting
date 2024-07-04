using UIGameDataManager;

public interface _ICharacterStatsFactory
{
    Stats CreateCharacterStats(int attack, int life, float attackSpeed, float specialAttack);
    _IStatIncreaseStrategy CreateStatIncreaseStrategy();
}
