using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillComponent : MonoBehaviour
{
    [SerializeField]
    ItemObject itemObject;
    public ItemObject ItemObject { get { return itemObject; } set { itemObject = value; } }


}
