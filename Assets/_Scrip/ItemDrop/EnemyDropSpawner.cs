using UnityEngine;

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
    public int[] itemsValue;
    protected override void Awake()
    {
        base.Awake();
        EnemyDropSpawner.instance = this;
    }
    public int testMaxValue;
    [ContextMenu("TestMaxValue")]
    public void TestMaxValue()
    {
        print("Test Started");
        var maxValue = GetMaxItemValue(testMaxValue);
        print("Test Completed ! Max Value:"+maxValue);
    }

    public int GetMaxItemValue(int total)
    {
        var itemsValue = new int[this.itemsValue.Length];
        for (int i = 0 ;i < itemsValue.Length; i++)         {
            itemsValue[i] = this.itemsValue[i];
        }
        var maxValue = int.MinValue;
        foreach(var value in itemsValue)
        {
            if (value > total) continue;
            if (value < maxValue) continue;
            maxValue = value;
        }
        return maxValue;
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
