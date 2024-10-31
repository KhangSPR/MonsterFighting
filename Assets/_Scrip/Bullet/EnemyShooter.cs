using UnityEngine;

public class EnemyShooter : BulletShooter
{
    protected override Vector3 GetShootingDirection()
    {
        return Vector3.left;
    }

    protected override bool IsShooting()
    {
        if (this.EnemyCtrl.EnemyAttack.CheckCanAttack)
            return true;
        else
            return false;
    }
}