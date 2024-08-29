
public class EnemyDropSpawner : Spawner
{
    private static EnemyDropSpawner instance;
    public static EnemyDropSpawner Instance { get => instance; }

    public static string DefaultDropItem = "ItemGem"; // Item 1
    public static string BossDropItem = "ItemGemBoss"; // Item 1
    public static string WolfFangs = "ItemWolfFangs"; // Item 1


    public int[] itemsValue = new int[] { 1, 3, 5, 7, 10 };

    protected override void Awake()
    {
        base.Awake();
        EnemyDropSpawner.instance = this;
    }
    public int GetMaxItemValue(int total)
    {
        var itemsValue = new int[this.itemsValue.Length];
        for (int i = 0; i < itemsValue.Length; i++)
        {
            itemsValue[i] = this.itemsValue[i];
        }
        var maxValue = int.MinValue;
        foreach (var value in itemsValue)
        {
            if (value > total) continue;
            if (value < maxValue) continue;
            maxValue = value;
        }
        return maxValue;
    }
    public string GetDropItemForEnemy(ItemDropType itemDropType)
    {
        switch (itemDropType)
        {
            case ItemDropType.MagicalCrystal:
                return DefaultDropItem;
            case ItemDropType.Crystalline:
                return BossDropItem;
            case ItemDropType.WolfFangs:
                return WolfFangs;
            default:
                return DefaultDropItem;
        }
    }
}
