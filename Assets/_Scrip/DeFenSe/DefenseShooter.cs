using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseShooter : BulletShooter
{
    [Header("Defense Shooter")]
    [SerializeField] protected float launchForce;
    public float LaunchForce => launchForce;
    protected override Vector3 GetShootingDirection()
    {
        return Vector3.right;
    }
    protected override bool IsShooting()
    {
        return this.deFenSeCtrl.DefenseLookAtDistance.enemy != null;
    }
}
