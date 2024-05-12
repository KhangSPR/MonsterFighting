using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInventoryUIAbstract : SaiMonoBehaviour
{
    public CardInventoryUICtrl CardInventoryUICtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCardInventoryUICtrl();
    }
    protected void LoadCardInventoryUICtrl()
    {
        if (this.CardInventoryUICtrl != null) return;
        this.CardInventoryUICtrl = transform.parent.GetComponent<CardInventoryUICtrl>();
        Debug.Log(gameObject.name + ": LoadCardInventoryUICtrl" + gameObject);
    }

}
