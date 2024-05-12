using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAppearWithoutRun : SaiMonoBehaviour, ObjAppearObserver
{
    [Header("Obj Appear WithoutShoot")]
    [SerializeField] protected ObjAppearing objAppearing;
    public ObjAppearing ObjAppearing => objAppearing;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.RegisterAppearEvent();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjAppearing();
    }
    protected virtual void LoadObjAppearing()
    {
        if (this.objAppearing != null) return;
        this.objAppearing = transform.GetComponent<ObjAppearing>();
        Debug.Log(gameObject.name + ": loadObjAppearing" + gameObject);
    }
    //Sign up for the event
    protected virtual void RegisterAppearEvent()
    {
        this.objAppearing.OnAppearAdd(this);
    }

    public void OnAppearStar()
    {

    }

    public void OnAppearFinish()
    {

    }
}
