using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBtn : SaiMonoBehaviour
{
    [SerializeField]
    protected Sprite sprite;
    public Sprite Sprite
    {
        get { return sprite; }
        set { sprite = value; }
    }
    public virtual GameObject CardPrefabInstance
    {
        get { return null; } // Default Null
    }

    // Price Card
    protected int price;
    public int Price { get { return price; } set { price = value; } }


    public abstract GameObject PlaceAbstract(Transform tileTransform);

}