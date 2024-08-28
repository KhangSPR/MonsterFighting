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
    public void CreateDisplayPlayByType(InventoryType type, Transform Holder, GameObject prefab)
    {
        // Xóa các đối tượng con cũ trong Holder
        foreach (Transform child in Holder)
        {
            Destroy(child.gameObject);
        }

        // Lọc các mục có type đúng
        List<InventorySlot> filteredSlots = inventory.Container.Items
            .Where(slot => slot.item != null && slot.item.type == type)
            .ToList();

        // Hiển thị các mục đã lọc
        foreach (var slot in filteredSlots)
        {
            if(slot.ID >= 0 )
            {
                var obj = Instantiate(prefab, Holder.position, Quaternion.identity, Holder);
                obj.transform.GetComponent<SkillComponent>().ItemObject = inventory.database.GetItem[slot.item.Id];
                obj.transform.Find("Icon").GetComponent<Image>().sprite = inventory.database.GetItem[slot.item.Id].Sprite;
                obj.transform.Find("Count").GetComponent<TMP_Text>().text = slot.amount == 0 ? "" : "x" + slot.amount.ToString();
            }
        }

        // In ra số lượng các mục đã lọc
        Debug.Log("CreateDisplayPlayByType: " + filteredSlots.Count);
    }
}
