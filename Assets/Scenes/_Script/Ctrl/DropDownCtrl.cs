using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownCtrl : CardInventoryUIAbstract
{
    public Transform FadeImage;
    public Fade fade;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObject();
        this.LoadFade();
    }
    protected void LoadFade()
    {
        if (fade != null) return;
        fade = transform.GetComponentInChildren<Fade>();
    }
    protected void LoadObject()
    {
        if (FadeImage != null) return;
        FadeImage = transform.Find("FadeImage");
        Debug.Log("LoadLoadObject");
    }
    public Transform GetOBJFade()
    {
        return FadeImage;
    }

}
