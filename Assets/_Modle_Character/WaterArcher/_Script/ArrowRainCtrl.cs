using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainCtrl : SkillCtrl
{
    [Space]
    [Header("Arrow Rain")]
    public int damageHit;
    [SerializeField] ParticleSystem ParticleSystem;
    [SerializeField] ParticleCollisionInstance collisionInstance;
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void SkillAction()
    {
        if (objectCtrl == null || ParticleSystem == null) return;

        HandleLandLifeTime(objectCtrl.ObjLand.LandIndex);
    }
    public override void SkillColider(ObjectCtrl objectCtrl)
    {
    }
    private void HandleLandLifeTime(int land)
    {
        var main = ParticleSystem.main;
        switch (land)
        {
            case 0:
                main.startLifetime = 0.2f;
                break;
            case 1:
                main.startLifetime = 0.25f;
                break;
            case 2:
                main.startLifetime = 0.3f;
                break;
        }
    }

}

