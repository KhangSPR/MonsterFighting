using UnityEngine;

public enum InventoryType
{
    Skill,
    Medicine,
    ItemCraft,
}
public enum Attributes
{
    Agility,
    Itellect,
    Stamina,
    Strength
}
public enum ItemRarity
{
    Common,
    Rare,
    Epic,
    Legendary
}
public abstract class ItemObject : ScriptableObject
{
    public bool IsUsed;
    public int Id;
    public Sprite Sprite;
    public InventoryType type;
    public string Name;
    [TextArea(15, 20)]
    public string description;
    public ItemRarity itemRarity;
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
[System.Serializable]
public class Item
{
    public string Name;
    public int Id;
    public InventoryType type;
    public bool IsUsed;

    public Item(ItemObject item)
    {
        Name = item.name;
        Id = item.Id;
        type = item.type;
        IsUsed = item.IsUsed;

    }

}

#region Comment
//public ItemBuff[] buff;

//public ItemBuff[] buffs;

//buffs = new ItemBuff[item.buff.Length];
//for (int i = 0; i < buffs.Length; i++)
//{
//    buffs[i] = new ItemBuff(item.buff[i].min, item.buff[i].max)
//    {
//        attributes = item.buff[i].attributes
//    };
//}
//[System.Serializable]
//public class ItemBuff
//{
//    public Attributes attributes;
//    public int value;
//    public int min;
//    public int max;
//    public ItemBuff(int _min, int _max)
//    {
//        min = _min; max = _max;
//        GenerateValue();
//    }
//    public void GenerateValue()
//    {
//        value = UnityEngine.Random.Range(min, max);
//    }
//}
#endregion