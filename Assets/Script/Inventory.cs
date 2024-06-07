using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Inventory
{
    public static Inventory instance;
    public static Inventory Instance => instance;
    public List<InventorySlot> ItemSlots = new ();
    public void AddOrChangeQuantity(Item item , int quantity)
    {
        var ItemSlot = ItemSlots.FirstOrDefault(itemSlot => itemSlot.item == item);
        if(ItemSlot!= null)
        {
            ItemSlot.count += quantity;
        } else
        {
            ItemSlots.Add(new InventorySlot(item, quantity));
        }
    }

    public void RemoveOrChangeQuantity(Item item, int quantity)
    {
        var ItemSlot = ItemSlots.FirstOrDefault(itemSlot => itemSlot.item == item);
        if (ItemSlot != null)
        {
            ItemSlot.count -= quantity;
            if (ItemSlot.count <= 0) ItemSlots.Remove(ItemSlot);
        }
    }
}
[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int count;

    public InventorySlot(Item item , int count)
    {
        this.item = item;
        this.count = count;
    }
}
