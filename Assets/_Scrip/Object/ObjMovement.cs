using UnityEngine;

public class ObjMovement : EnemyAbstract
{
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int directionMove;
    public void Move()
    {
        transform.parent.Translate(transform.right* directionMove * this.moveSpeed * Time.fixedDeltaTime);
    }

}
