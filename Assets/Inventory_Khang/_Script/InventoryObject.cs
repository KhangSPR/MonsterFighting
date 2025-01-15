using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

public class InventoryObject : ScriptableObject
{
    public string savePath;
    public Inventory Container;
    public ItemDatabaseObject database;
    //    private void OnEnable()
    //    {
    //#if UNITY_EDITOR
    //        database = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(ItemDatabaseObject));
    //#else
    //    database = Resources.Load<ItemDatabaseObject>("Database");
    //#endif
    //    }
    public void AddRandomItem()
    {
        ItemObject randomItemObject = database.GetRandomItem();
        Item randomItem = new Item(randomItemObject);
        int randomAmount = UnityEngine.Random.Range(1, 2); // số lượng ngẫu nhiên
        AddItem(randomItem, randomAmount);
    }
    public void AddItem(Item _item, int _amount)
    {
        //if (_item.buffs.Length > 0)
        //{
        //    Debug.Log("Item Buff");
        //    SetEmptySlot(_item, _amount);
        //    return;
        //}
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id && Container.Items[i].item.type == _item.type)
            {
                Debug.Log("Item notBuff");
                Container.Items[i].AddAmount(_amount);
                return;
            }
        }
        SetEmptySlot(_item, _amount);
    }
    // Hàm RemoveItem
    public void RemoveItem(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == _item.Id && Container.Items[i].item.type == _item.type)
            {
                if (Container.Items[i].amount >= _amount)
                {
                    Container.Items[i].amount -= _amount;
                    if (Container.Items[i].amount == 0)
                    {
                        Container.Items[i].UpdateSlot(-1, null, 0, false);
                    }
                    return;
                }
                else
                {
                    Debug.LogWarning("Not enough items to remove");
                    return;
                }
            }
        }
        Debug.LogWarning("Item not found in inventory");
    }
    private void SetEmptySlot(Item _item, int _amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID <= -1)
            {
                Container.Items[i] = new InventorySlot(_item.Id, _item, _amount);
                return;
            }
        }
    }
    #region Swap-SaveItem
    private void SwapItemSlotItemObjStarGame()
    {
        var sortedItems = Container.Items;

        for (int i = 0; i < sortedItems.Length; i++)
        {
            InventorySlot slot = sortedItems[i];


            if (slot.ID >= 0)
            {
                foreach (var databaseItem in database.Items)
                {
                    if (databaseItem.IdDatabase == slot.item.Id)
                    {
                        databaseItem.IsUsed = slot.item.IsUsed;

                    }
                }
            }
        }
    }
    private void SwapItemOBjItemSlotEndGame()
    {
        var sortedItems = Container.Items;

        for (int i = 0; i < sortedItems.Length; i++)
        {
            InventorySlot slot = sortedItems[i];


            if (slot.ID >= 0)
            {
                foreach(var databaseItem in database.Items)
                {
                    if(databaseItem.IdDatabase == slot.item.Id)
                    {
                        slot.item.IsUsed = databaseItem.IsUsed;

                    }
                }
            }
        }
    }
    #endregion
    [ContextMenu("Save")]
    public void Save()
    {
        SwapItemOBjItemSlotEndGame();

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
        Debug.Log("SAVE");
    }
    [ContextMenu("Load")]
    public void Load()
    {
        SwapItemSlotItemObjStarGame();

        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            // Tạo danh sách tạm thời để lưu các item có amount > 0
            List<InventorySlot> validItems = new List<InventorySlot>();

            for (int i = 0; i < Container.Items.Length; i++)
            {
                // Cập nhật slot
                Container.Items[i].UpdateSlot(newContainer.Items[i].ID, newContainer.Items[i].item, newContainer.Items[i].amount, newContainer.Items[i].item.IsUsed);

                // Chỉ thêm các item có amount > 0 vào danh sách
                if (Container.Items[i].amount > 0)
                {
                    validItems.Add(Container.Items[i]);

                }
            }
            // Sao chép các item hợp lệ vào mảng mới
            for (int i = 0; i < validItems.Count; i++)
            {
                Container.Items[i] = validItems[i];
            }

            Debug.Log("LOAD");
            stream.Close();
        }
    }



    [ContextMenu("Clear")]

    public void Clear()
    {
        Container = new Inventory();
    }
    #region Craft UI
    public int GetCountById(int id)
    {
        var sortedItems = Container.Items;

        foreach (var inventorySlot in sortedItems)
        {
            InventorySlot slot = inventorySlot;

            if (slot.ID >= 0)
            {
                foreach (var databaseItem in database.Items)
                {
                    if (databaseItem.IdDatabase == slot.item.Id)
                    {
                        if (databaseItem.IdDatabase == id)
                        {
                            return slot.amount;
                        }
                    }
                }
            }
        }
        return 0;
    }
    public ItemObject[] GetItemObjectCanCraftByID(string ID)
    {
        List<ItemObject> itemObjects = new List<ItemObject>();

        foreach (ItemObject itemObject in database.Items)
        {
            foreach (ItemRequiredCraft itemRequiredCraft in itemObject.itemRequiredCrafts)
            {
                if (!string.IsNullOrEmpty(itemRequiredCraft.ID) &&
                    string.Equals(itemRequiredCraft.ID.Trim(), ID.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    itemObjects.Add(itemObject);
                }
            }
        }

        return itemObjects.ToArray();
    }



    public ItemObject GetItemByID(string ID)
    {
        // Log tổng số Items trong database
        Debug.Log($"Total Items in database: {database.Items.Length}");

        foreach (ItemObject itemObject in database.Items)
        {
            // Log thông tin của ItemObject hiện tại
            Debug.Log($"Checking ItemObject: {itemObject.Name} (ID: '{itemObject.ID}', Length: {itemObject.ID?.Length ?? 0})");

            if (!string.IsNullOrEmpty(itemObject.ID) &&
                string.Equals(itemObject.ID.Trim(), ID.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                // Log khi tìm thấy ItemObject phù hợp
                Debug.Log($"Match found! Returning ItemObject: {itemObject.Name} (ID: {itemObject.ID})");
                return itemObject;
            }
        }

        // Log khi không tìm thấy ItemObject nào phù hợp
        Debug.Log($"No ItemObject found with ID: '{ID}'");
        return null;
    }

    #endregion
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[40];

}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public Item item;
    public int amount;
    public InventorySlot()
    {
        ID = -1;
        item = null;
        amount = 0;
    }

    public InventorySlot(int _id, Item _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void UpdateSlot(int _id, Item _item, int _amount, bool _isUsed) //Repair
    {
        ID = _id;
        item = _item;

        if (_item != null)
        {
            amount = _amount;
            item.IsUsed = _isUsed;
        }
        else
        {
            // Nếu _item là null, đặt các giá trị mặc định
            amount = 0;
            Debug.LogWarning($"UpdateSlot called with a null item. ID: {_id}, Amount set to 0.");
        }
    }


    public void AddAmount(int value)
    {
        amount += value;
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemObject itemObject;
    public int count;

    public InventoryItem(ItemObject itemObject, int count)
    {
        this.itemObject = itemObject;
        this.count = count;
    }
}
#region Comment
//try
//{
//    Debug.Log("Save");

//    string saveData = JsonUtility.ToJson(this, true);
//    BinaryFormatter bf = new BinaryFormatter();
//    string fullPath = Path.Combine(Application.persistentDataPath, savePath);
//    FileStream file = File.Create(fullPath);
//    bf.Serialize(file, saveData);
//    file.Close();

//    Debug.Log($"Saved data to {fullPath}");
//}
//catch (Exception ex)
//{
//    Debug.LogError($"Failed to save data: {ex.Message}");
//}
#endregion
#region Comment
//try
//{
//    string fullPath = Path.Combine(Application.persistentDataPath, savePath);
//    if (File.Exists(fullPath))
//    {
//        Debug.Log("Load");

//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Open(fullPath, FileMode.Open);
//        JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
//        file.Close();

//        Debug.Log($"Loaded data from {fullPath}");
//    }
//    else
//    {
//        Debug.LogWarning("Save file does not exist.");
//    }
//}
//catch (Exception ex)
//{
//    Debug.LogError($"Failed to load data: {ex.Message}");
//}
#endregion
