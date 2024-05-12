using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardShooter : BulletShooter
{
    protected override Vector3 GetShootingDirection()
    {

        return Vector3.right;
    }
    protected override bool IsShooting()
    {
        return this.guardCtrl.ObjLookAtEnemy.target != null;
    }

}
