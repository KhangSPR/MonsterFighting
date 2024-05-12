using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class TileMachine : TileScrip
{
    private Color32 fullColor = new Color32(255, 118, 118, 255);
    private Color32 emptyColor = new Color32(96, 255, 90, 255);

    private SpriteRenderer spriteRenderer;

    protected override void OnMouseExit()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnMouseOver()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //protected override void OnMouseOver()
    //{
    //    ColorTile(fullColor);
    //    if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickBtn != null && GameManager.Instance.ClickBtn is TowerBtn)
    //    {
    //        if (IsEmpty)
    //        {
    //            ColorTile(emptyColor);
    //        }
    //        if (!IsEmpty)
    //        {
    //            ColorTile(fullColor);
    //        }
    //        else if (Input.GetMouseButtonDown(0))
    //        {
    //            Place(transform);

    //            //Enable Sprite
    //            Hover.Instance.Deactivate();

    //            PanelManager.Instance.TogglePanelID();

    //            PanelManager.Instance.ToggleCancel();


    //        }
    //    }
    //}

    //protected override void OnMouseExit()
    //{
    //    ColorTile(UnityEngine.Color.white);
    //}
    private void ColorTile(UnityEngine.Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
