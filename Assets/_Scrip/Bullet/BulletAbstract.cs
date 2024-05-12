using UnityEngine;

public abstract class BulletAbstract : SaiMonoBehaviour
{
    [Header("Bullet Abtract")]
    [SerializeField] protected BulletRegularCtrl bulletRegularCtrl;
    public BulletRegularCtrl BulletRegularCtrl { get => bulletRegularCtrl; }
    [SerializeField] protected BulletExplodeCtrl bulletExplodeCtrl;
    public BulletExplodeCtrl BulletExplodeCtrl { get => bulletExplodeCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBulletExplodeCtrl();
        this.LoadBulletRegularCtrl();
    }

    protected virtual void LoadBulletExplodeCtrl()
    {
        if (this.bulletExplodeCtrl != null) return;
        this.bulletExplodeCtrl = transform.parent.GetComponent<BulletExplodeCtrl>();
        Debug.Log(transform.name + ": LoadBulletCtrl", gameObject);
    }
    protected virtual void LoadBulletRegularCtrl()
    {
        if (this.bulletRegularCtrl != null) return;
        this.bulletRegularCtrl = transform.parent.GetComponent<BulletRegularCtrl>();
        Debug.Log(transform.name + ": LoadBulletCtrl", gameObject);
    }
    public abstract BulletCtrl GetBulletCtrl();
}
