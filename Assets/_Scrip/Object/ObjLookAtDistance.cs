using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetTag
{
    Enemy,
    Player,
    Boss,
}
public class ObjLookAtDistance : ObjLookAtTarget
{
    [Header("LookAt Distance Target")]
    public Transform target;
    [SerializeField] protected TargetTag listTag;
    [SerializeField] protected float maxSearchDistance = 5f; // Max Distance

    protected override void FixedUpdate()
    {
        this.FindClosestEnemy();
        this.GetTargetPosition();
        base.FixedUpdate();
    }
    protected virtual void FindClosestEnemy()
    {
        GameObject enemies = GameObject.FindGameObjectWithTag(listTag.ToString());

        if (enemies == null) return;

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        float distance = Vector3.Distance(transform.position, enemies.transform.position);
        if (distance < closestDistance && distance <= maxSearchDistance)
        {
            closestDistance = distance;
            closestEnemy = enemies.transform;
        }

        target = closestEnemy;
    }

    protected virtual void GetTargetPosition() //Hàm con trỏ chuột
    {
        if (this.target == null) return;

        this.targetPosition = this.target.transform.position;
        this.targetPosition.z = 0;
    }
}
