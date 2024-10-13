using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileTower : TileScrip
{
    private void IsActive()
    {
        if (newObjSet == null) return;

        if(!newObjSet.activeSelf)
        {
            ActionPlace();
        }
    }
    private void DeActive()
    {
        
    }
    private void ActionPlace()
    {
        Place(transform);

        GameManager.Instance.BuyCard();

        GameManager.Instance.CardRefresh.StartCooldown();
    }

    protected override void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null && GameManager.Instance.ClickBtn is CardButton)
        {
            if (!IsEmpty && Input.GetMouseButtonDown(0))
            {
                IsActive();
            }
            else if (Input.GetMouseButtonDown(0) && IsEmpty)
            {
                ActionPlace();
            }
        }
    }

    protected override void OnMouseExit()
    {
        DeActive();
    }
}
