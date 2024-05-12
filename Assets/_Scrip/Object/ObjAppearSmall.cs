using System.Collections;
using System.Collections.Generic;
using UIGameDataMap;
using UnityEngine;

public class ObjAppearSmall : ObjAppearing
{
    [Header("Obj Appear Smaller")]
    [SerializeField] protected float speedScale = 0.01f;
    [SerializeField] protected float currentScale = 1f;
    [SerializeField] protected float minScale = 0.1f;
    [SerializeField] protected float maxScale = 1f;

    [Header("Portal")]
    [SerializeField] Transform darkBeam;
    [SerializeField] Transform particle;


    protected override void OnEnable()
    {
        base.OnEnable();
        InitScale();
    }

    protected override void AppeaRing()
    {
        this.currentScale -= this.speedScale;

        transform.parent.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
        darkBeam.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
        particle.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);

        if (this.currentScale <= this.minScale)
            Appear();
    }

    public virtual void InitScale()
    {
        this.appeared = false;
        this.isAppearing = false;
        this.currentScale = this.maxScale;
        transform.parent.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
        darkBeam.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
        particle.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
    }

    protected override void Appear()
    {
        base.Appear();
        transform.parent.localScale = new Vector3(this.minScale, this.minScale, this.minScale);
        darkBeam.localScale = new Vector3(this.minScale, this.minScale, this.minScale);
        particle.localScale = new Vector3(this.minScale, this.minScale, this.minScale);
    }

    public override bool ShouldCallAppearing()
    {
        if (!isAppearing)
            return false;
        else
            return true;
   
    }
}
