public enum EnemyTag
{
    Default,
    Boss
}

public class EnemyDropSpawner : Spawner
{
    private static EnemyDropSpawner instance;
    public static EnemyDropSpawner Instance { get => instance; }

    public static string DefaultDropItem = "ItemGem"; // Item 1
    public static string BossDropItem = "ItemGemBoss"; // Item 1

    protected override void Awake()
    {
        base.Awake();
        EnemyDropSpawner.instance = this;
    }

    public string GetDropItemForEnemy(EnemyTag tag)
    {
        switch (tag)
        {
            case EnemyTag.Default:
                return DefaultDropItem;
            case EnemyTag.Boss:
                return BossDropItem;
            default:
                return DefaultDropItem;
        }
    }
}
