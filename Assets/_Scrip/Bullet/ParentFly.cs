using UnityEngine;

public class ParentFly : BulletAbstract
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Vector3 bulletDirection = Vector3.right;
    [SerializeField] protected Transform ParentObject;

    public override BulletCtrl GetBulletCtrl()
    {
        throw new System.NotImplementedException();
    }
    //Direction Bullet
    protected override void Update()
    {
        base.Update();
        transform.parent.Translate(this.bulletDirection * this.moveSpeed * Time.deltaTime);

    }
    // Gọi từ bên ngoài khi có va chạm từ bên trái  
}