using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoisonType
{
    Heal,
    Power,
    Magic,
    Shield,

}
[CreateAssetMenu(fileName = "New Poison Object", menuName = "Inventory System/Items/Poison")]
public class PoisonObject : ItemObject
{
    public PoisonType poisonType;
    public int addPoint;
    public GameObject gameobjectVFX;


    public void Awake()
    {
        type = ItemType.Poison;
    }
}
