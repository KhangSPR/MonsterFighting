using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAppearBigger : ObjAppearing
{
    [Header("Obj Appear Bigger")]
    [SerializeField] protected float speedScale = 0.01f;
    [SerializeField] protected float currentScale = 0f;
    [SerializeField] protected float minScale = 0.1f;
    [SerializeField] protected float maxScale = 1f;
    protected override void OnEnable()
    {
        base.OnEnable();

        this.InitScale();

    }
    protected override void AppeaRing()
    {

        this.currentScale += this.speedScale;

        transform.parent.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
        if (this.currentScale >= this.maxScale) this.Appear();


    }
    public virtual void InitScale()
    {
        this.currentScale = this.minScale;
        transform.parent.localScale = new Vector3(this.currentScale, this.currentScale, this.currentScale);
    }
    protected override void Appear()
    {
        base.Appear();
        transform.parent.localScale = new Vector3(this.maxScale, this.maxScale, this.maxScale);
    }

    public override bool ShouldCallAppearing()
    {
        return true;
    }

}
