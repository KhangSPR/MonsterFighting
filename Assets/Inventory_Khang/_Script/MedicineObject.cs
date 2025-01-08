using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Medicine
{
    Heal,
    Power,
    Magic,
    Shield,

}
[CreateAssetMenu(fileName = "New Poison Object", menuName = "Inventory System/Items/Poison")]
public class MedicineObject : ItemObject
{
    public Medicine medicineType;
    public int addPoint;
    public GameObject gameobjectVFX;
    public int coolDown;
    public void Awake()
    {
        type = InventoryType.Medicine;
    }
}
