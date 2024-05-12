using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Script này sau này sẽ lưu vào json
[CreateAssetMenu]
public class Castle_Shop : ScriptableObject
{
    public List<Castle_Shop_Item> csi = new List<Castle_Shop_Item>();
    public CastleSO choosen_item;
}
