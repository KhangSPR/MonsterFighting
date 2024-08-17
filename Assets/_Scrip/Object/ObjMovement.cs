using UnityEngine;

public class ObjMovement : EnemyAbstract
{
    [SerializeField] protected float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }
    [SerializeField] protected int directionMove;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.moveSpeed = enemyCtrl.EnemySO.basePointsSpeedMove;
        Debug.Log("Onginal: " + enemyCtrl.EnemySO.basePointsSpeedMove);
    }
    public void Move()
    {
        transform.parent.Translate(transform.right* directionMove * this.moveSpeed * Time.fixedDeltaTime);
    }

}
