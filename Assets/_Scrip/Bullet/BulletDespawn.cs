using UnityEngine;

public class BulletDespawn : DespawnByTime
{
    //[SerializeField] protected BulletCtrl bulletCtrl;
    //public BulletCtrl BulletCtrl { get => bulletCtrl; }
    //protected override void LoadComponents()
    //{
    //    base.LoadComponents();
    //    this.loadBulletCtrl();
    //}
    //protected virtual void loadBulletCtrl()
    //{
    //    if (this.bulletCtrl != null) return;
    //    this.bulletCtrl = transform.parent.GetComponent<BulletCtrl>();
    //    Debug.Log(gameObject.name + ": loadBulletCtrl" + gameObject);
    //}
    public override void deSpawnObjParent()
    {

        //if (bulletCtrl is BulletExplodeCtrl)
        //{
        //    // BulletCtrl là kiểu BulletExplodeCtrl
        //    // Thực hiện các xử lý liên quan đến BulletExplodeCtrl ở đây
        //    BulletSpawner.Instance.Despawn(transform);
        //}
        //else
        //{
        //    // BulletCtrl không phải là kiểu BulletExplodeCtrl (có thể là Regular)
        //    // Thực hiện các xử lý tương ứng ở đây
            BulletSpawner.Instance.Despawn(transform.parent);
        //}
    }
}
