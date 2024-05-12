using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Castle_Shop_Item",fileName = "Castle_Shop_Item")]
public class Castle_Shop_Item :ScriptableObject
{
    public CastleSO castle;
    public int cost;
}
