using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileTower : TileScrip
{
    private void IsActive()
    {

    }
    private void DeActive()
    {
        
    }

    protected override void OnMouseOver()
    {
        IsActive();
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null && GameManager.Instance.ClickBtn is CardButton)
        {
            if (IsEmpty)
            {
                IsActive();
            }
            if (!IsEmpty)
            {

            }
            else if (Input.GetMouseButtonDown(0))
            {
                Place(transform);

                GameManager.Instance.BuyCard();

                GameManager.Instance.CardRefresh.StartCooldown();
            }
        }
    }

    protected override void OnMouseExit()
    {
        DeActive();
    }
}
