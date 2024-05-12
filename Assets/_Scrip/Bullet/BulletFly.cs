using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFly : ParentFly
{

    protected override void OnEnable()
    {
        base.OnEnable();
        if (bulletRegularCtrl != null)
        {
            bulletRegularCtrl.OnSetBullet += OnSetBullet;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        if (bulletRegularCtrl != null)
        {
            bulletRegularCtrl.OnSetBullet -= OnSetBullet;
        }
    }
    protected override void loadValue()
    {
        base.loadValue();
        // Get the BulletCtrl instance and set the moveSpeed property
        BulletCtrl bulletCtrl = GetBulletCtrl();
        if (bulletCtrl != null)
        {
            this.moveSpeed = bulletCtrl.ShootAbleObjectSO.speedFly;
        }
    }
    public override BulletCtrl GetBulletCtrl()
    {
        // Depending on your hierarchy, you might need to adjust this.
        // Here, it assumes that BulletCtrl is a component of the parent GameObject.
        return transform.parent.GetComponent<BulletCtrl>();
    }
    void OnSetBullet()
    {
        if (bulletRegularCtrl.GetDirection() == Vector3.left)
        {
            bulletDirection = Vector3.left;
            ParentObject.localScale = new Vector3(-1 , 1, 1);

            Debug.Log("Left");

        }
        else
        {
            Debug.Log("Right");
            bulletDirection = Vector3.right;
            ParentObject.localScale = new Vector3(1, 1, 1);

        }
    }
}
