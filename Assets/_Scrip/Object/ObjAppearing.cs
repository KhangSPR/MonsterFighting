using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjAppearing : SaiMonoBehaviour
{
    [Header("Obj Appearing")]
    [SerializeField] protected bool isAppearing = false;
    public bool IsAppearing { get { return isAppearing; } set { isAppearing = value; } }
    [SerializeField] protected bool appeared = false;
    public bool Appeared { get { return appeared; } set { appeared = value; } }
    [SerializeField] protected List<ObjAppearObserver> Observer = new List<ObjAppearObserver>();
    protected override void Start()
    {
        base.Start();
        this.OnAppearStar();
    }
    protected virtual void FixedUpdate()
    {
        if (ShouldCallAppearing())
        {
            this.AppeaRing();
        }
    }
    protected abstract void AppeaRing();
    protected virtual void Appear()
    {
        this.appeared = true;
        this.isAppearing = false;
        //this.OnAppearFinish();
    }
    //since list is protected, use this function
    public virtual void OnAppearAdd(ObjAppearObserver Observer)
    {
        this.Observer.Add(Observer);
    }
    protected virtual void OnAppearStar()
    {
        foreach (ObjAppearObserver observer in this.Observer)
        {
            observer.OnAppearStar();
        }
    }
    protected virtual void OnAppearFinish()
    {
        foreach (ObjAppearObserver observer in this.Observer)
        {
            observer.OnAppearFinish();
        }
    }
    public abstract bool ShouldCallAppearing();
}
