using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class FollowTarget : SaiMonoBehaviour
{
    [SerializeField] protected Transform target;
    [SerializeField] protected float speedFollow = 4f;
    protected virtual void FixedUpdate()
    {
        this.followTarget();
    }
    protected virtual void followTarget()
    {
        if (this.target == null) return;
        transform.position = Vector3.Lerp(transform.position, this.target.position, Time.fixedDeltaTime * this.speedFollow);
    }
    public virtual void SetTarget(Transform target)
    {
        this.target = target;
    }
    public virtual Transform GetTarget()
    {
        return target;
    }
}
