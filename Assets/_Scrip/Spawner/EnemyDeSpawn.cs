using UnityEngine;
public class EnemyDeSpawn : DespawnByTime
{
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl => enemyCtrl;
    [SerializeField] protected bool CanDespawnTest = false;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadEnemyCtrl();
    }
    protected virtual void loadEnemyCtrl()
    {
        if (this.enemyCtrl != null) return;
        this.enemyCtrl = transform.parent.GetComponent<EnemyCtrl>();
        Debug.Log(gameObject.name + ": loadEnemyCtrl" + gameObject);
    }
    protected override void deSpawnObjParent()
    {
        EnemySpawner.Instance.Despawn(transform.parent);

        enemyCtrl.AbstractModel.DameFlash.SetMaterialDamageFlash();

        //Drop Item : rate, itemType, min max count
        enemyCtrl.EnemyDropItem.DropItem();
   
        //Drop Coin:
        enemyCtrl.EnemyDropCoin.DropCoin();
    }
    protected override bool canDespawn()
    {
        if (CanDespawnTest)
            return false;

        // Gọi base và trả về giá trị
        return base.canDespawn();
    }
}
