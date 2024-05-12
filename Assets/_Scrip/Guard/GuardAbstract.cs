using UnityEngine;

public abstract class GuardAbstract : SaiMonoBehaviour
{
    [Header("Guard Abtract")]
    [SerializeField] protected GuardCtrl guardCtrl;
    public GuardCtrl GuardCtrl { get => guardCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGuardCtrl();
    }

    protected virtual void LoadGuardCtrl()
    {
        if (this.guardCtrl != null) return;
        this.guardCtrl = transform.parent.GetComponent<GuardCtrl>();
        Debug.Log(transform.name + ": LoadGuardCtrl", gameObject);
    }
}
