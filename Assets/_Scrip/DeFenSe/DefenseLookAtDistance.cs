using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseLookAtDistance : SaiMonoBehaviour
{

    [Header("LookAt Distance Enemy")]
    public Transform enemy;
    [SerializeField] protected string targetTag = "Enemy"; // Tag của target
    [SerializeField] protected float maxSearchDistance = 5f; // Khoảng cách tìm kiếm tối đa
    //private static ObjLookAtDistanceGuard instance;
    //public static ObjLookAtDistanceGuard Instance { get => instance; }
    //protected override void Awake()
    //{
    //    base.Awake();
    //    if (ObjLookAtDistanceGuard.instance != null) Debug.LogError("Onlly 1 ObjLookAtDistanceGuard Warning");
    //    ObjLookAtDistanceGuard.instance = this;
    //}
    protected override void Update()
    {
        base.Update();
        this.FindClosestEnemy();
    }
    protected virtual void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemyObject in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemyObject.transform.position);
            if (distance < closestDistance && distance <= maxSearchDistance)
            {
                closestDistance = distance;
                closestEnemy = enemyObject.transform;
            }
        }

        enemy = closestEnemy;
    }
}
