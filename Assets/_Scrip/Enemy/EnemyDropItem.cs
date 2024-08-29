using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDropItem : EnemyAbstract
{
    private int[] minDropItemCount;
    private int[] maxDropItemCount;
    private float[] spawnRate;
    private ItemDropType[] dropTypes;

    private int countItemDrop;

    
    protected override void OnEnable()
    {
        base.OnEnable();
        SetEnemyRate();
    }
    public void DropItem()
    {
        for (int i = 0; i < countItemDrop; i++)
        {
            var rate = Random.Range(0f, 100f);

            Debug.Log("Rate: " + rate);

            int itemValueTotal = Random.Range(minDropItemCount[i], maxDropItemCount[i] + 1);
            if (rate <= spawnRate[i])
            {
                while (itemValueTotal > 0)
                {
                    Transform newDropItem = EnemyDropSpawner.Instance.Spawn(GetTag(dropTypes[i]), transform.parent.position, Quaternion.identity);

                    ItemDropCtrl itemDropCtrl = newDropItem.GetComponent<ItemDropCtrl>();

                    //SET ITEM RARE, DROPING

                    if (itemDropCtrl is ItemDropDisplay)
                    {
                        ItemDropDisplay itemDropDisplay = (ItemDropDisplay)itemDropCtrl;

                        itemDropDisplay.ItemDropType = dropTypes[i];
                        itemDropDisplay.CountItem = itemValueTotal;

                    }
                    else
                    {
                        ItemDropInventory itemDropInventory = (ItemDropInventory)itemDropCtrl;

                        // Gán lại bản sao đã thay đổi cho thuộc tính
                        itemDropInventory.InventoryItem.count = itemValueTotal;
                    }


                    newDropItem.gameObject.SetActive(true);
                    var maxValue = EnemyDropSpawner.Instance.GetMaxItemValue(itemValueTotal);
                    itemValueTotal -= maxValue;

                    newDropItem.name = "Drop Item Value :" + maxValue;

                    switch (maxValue)
                    {
                        case 1: { newDropItem.localScale *= 1f; break; }
                        case 3: { newDropItem.localScale *= 1.2f; break; }
                        case 5: { newDropItem.localScale *= 1.5f; break; }
                        case 7: { newDropItem.localScale *= 1.7f; break; }
                        case 10: { newDropItem.localScale *= 2f; break; }
                    }
                    Debug.Log("Drop Item Value: " + maxValue + " by :" + transform.parent.name, transform.parent);
                }
            }
        }
    }

    private void SetEnemyRate()
    {
        ItemDrop[] itemDrop = enemyCtrl.EnemySO.itemDrop;

        countItemDrop = itemDrop.Length;

        // Khởi tạo các mảng với kích thước bằng với itemDrop.Length
        minDropItemCount = new int[countItemDrop];
        maxDropItemCount = new int[countItemDrop];
        spawnRate = new float[countItemDrop];
        dropTypes = new ItemDropType[countItemDrop];

        for (int i = 0; i < itemDrop.Length; i++)
        {
            minDropItemCount[i] = itemDrop[i].minDrop;
            maxDropItemCount[i] = itemDrop[i].maxDrop;
            spawnRate[i] = itemDrop[i].SpawnRate / 100f;
            dropTypes[i] = itemDrop[i].itemDropType;
        }

        Debug.Log("Goi 1 lan SetEnemyRate");
    }

    private string GetTag(ItemDropType itemDropType)
    {
        return EnemyDropSpawner.Instance.GetDropItemForEnemy(itemDropType);
    }
}
