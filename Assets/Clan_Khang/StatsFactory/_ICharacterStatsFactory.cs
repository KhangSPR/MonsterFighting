using UIGameDataManager;

public interface _ICharacterStatsFactory
{
    Stats CreateCharacterStats(int attack, int life, int deff, float attackSpeed, float specialAttack, float attackSpeedMelee, float recoverMana);
    _IStatIncreaseStrategy CreateStatIncreaseStrategy();
}
