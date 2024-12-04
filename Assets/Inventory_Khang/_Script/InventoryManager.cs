using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public InventoryObject inventory;
    public DisplayInventory displayInventory;

    //Limited Quantity
    private int limitedQuantityItem = 5;
    public int LimitedQuantityItem => limitedQuantityItem;
    [SerializeField]
    private int currentQuantityItem = 0;
    public int CurrentQuantityItem
    {
        get { return currentQuantityItem; }
        set
        {
            if (value > limitedQuantityItem)
            {
                currentQuantityItem = limitedQuantityItem;
            }
            else if (value < 0)
            {
                currentQuantityItem = 0;
            }
            else
            {
                currentQuantityItem = value;
            }
        }
    }
    public void SetCurrentQuantityItem(InventoryObject inventoryObject)
    {
        if (inventoryObject == null) return;

        var sortedItems = inventory.Container.Items;

        for (int i = 0; i < sortedItems.Length; i++)
        {
            InventorySlot slot = sortedItems[i];


            if (slot.ID >= 0)
            {
                if (slot.item.IsUsed)
                {
                    if (currentQuantityItem > 5) return;
                    currentQuantityItem++;
                }
            }
        }
    }

    private static InventoryManager instance;
    public static InventoryManager Instance => instance;
    protected void Awake()
    {
        if (InventoryManager.instance != null)
        {
            Debug.LogError("Only 1 InventoryManager Warning");
        }
        InventoryManager.instance = this;
    }

    private void Start()
    {
        inventory.Load();
        inventory.Load();

        //Quantity Item
        this.SetCurrentQuantityItem(inventory);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            inventory.Load();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddRandomItem();
            displayInventory.CreateDisplay();
            inventory.Save();
        }
    }
    private void OnApplicationQuit()
    {

        inventory.Save();
        inventory.Container.Items = new InventorySlot[40];
    }
    //Create Item InGame
    public void CreateDisplayPlayByType(Transform Holder, GameObject prefab)
    {
        // Xóa các đối tượng con cũ trong Holder
        foreach (Transform child in Holder)
        {
            Destroy(child.gameObject);
        }
        var sortedItems = inventory.Container.Items;

        foreach (var inventorySlot in sortedItems)
        {
            InventorySlot slot = inventorySlot;

            // Hiển thị các mục đã lọc
            foreach (var databaseItem in inventory.database.Items)
            {
                if(slot.ID >=0)
                {
                    if (databaseItem.IsUsed && databaseItem.Id == slot.item.Id)
                    {
                        var obj = Instantiate(prefab, Holder.position, Quaternion.identity, Holder);
                        obj.transform.GetComponent<SkillComponent>().ItemObject = databaseItem;
                        obj.transform.Find("Icon").GetComponent<Image>().sprite = databaseItem.Sprite;
                        obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
                    }
                }
            }

        }
    }
}
