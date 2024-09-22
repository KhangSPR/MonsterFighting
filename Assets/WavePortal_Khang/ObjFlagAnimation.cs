using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjFlagAnimation : SaiMonoBehaviour
{
    [SerializeField] Animator animator;

    bool flag = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnimator();
    }
    protected void LoadAnimator()
    {
        if (animator != null) return;
        this.animator = GetComponent<Animator>();
        animator.enabled = false;

    }
    public void RunAnimationFlag()
    {
        if (flag) return;
        //animator
        animator.enabled = true;
        flag = true;

    }
}
