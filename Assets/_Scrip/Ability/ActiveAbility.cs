using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveAbility : BaseAbility
{
    [Header("Active Ability")]
    [SerializeField] protected float timer = 2f;
    [SerializeField] protected float delay = 2f;
    [SerializeField] protected bool isReady = false;

    [SerializeField]
    protected bool checkALLEnemyDead = false;
    public bool CheckAllEnemyDead => checkALLEnemyDead;
    protected virtual void FixedUpdate()
    {
        if (!GameManager.Instance.ReadyTimer) return;
        if (CheckAllEnemyDead)
        {
            return;
        }
        this.Timing();
    }
    protected virtual void Timing()
    {
        if (this.isReady) return;
        this.timer += Time.fixedDeltaTime;
        if (this.timer < this.delay) return;
        this.isReady = true;
    }

    public virtual void Active()
    {
        this.isReady = false;
        this.timer = 0;
    }
    public float RandomRange(float min, float max)
    {
        return Random.Range(min, max + 1);
    }
}
