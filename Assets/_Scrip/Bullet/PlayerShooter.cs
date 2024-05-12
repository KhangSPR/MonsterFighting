using UnityEngine;

public class PlayerShooter : BulletShooter
{
    protected override Vector3 GetShootingDirection()
    {
        return Vector3.right;
    }

    protected override bool IsShooting()
    {
        if (this.PlayerCtrl.PlayerAttack.canAttack)
            return true;
        else
            return false;
    }
}