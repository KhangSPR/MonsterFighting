using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyDeSpawn : DespawnByTime
{
    [SerializeField] protected EnemyCtrl enemyCtrl;
    [SerializeField,Range(0,100)] private float SpawnRate;
    [SerializeField] private float rate;
    [SerializeField] private float startItemValueTotal;
    public EnemyCtrl EnemyCtrl => enemyCtrl;
    public int minDropItemCount;
    public int maxDropItemCount;
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
    public override void deSpawnObjParent()
    {
        Debug.Log("Enemy Despawn");
        EnemySpawner.Instance.Despawn(transform.parent);
        var rate = Random.Range(0f, 100f);
        var itemValueTotal = Random.Range(minDropItemCount, maxDropItemCount + 1);
        startItemValueTotal = itemValueTotal;
        if(rate <= SpawnRate)
        {
            while(itemValueTotal > 0)
            {
                Transform newDropItem = EnemyDropSpawner.Instance.Spawn(GetTag(), transform.parent.position, Quaternion.identity);

                newDropItem.gameObject.SetActive(true);
                var maxValue = EnemyDropSpawner.Instance.GetMaxItemValue(itemValueTotal);
                itemValueTotal -= maxValue;
                newDropItem.name = "Drop Item Value :" + maxValue;
                switch (maxValue)
                {
                    case 1: { newDropItem.localScale *= 0.5f; break; }
                    case 5: { newDropItem.localScale *= 0.8f; break; }
                    case 10: { newDropItem.localScale *= 1f; break; }
                    case 15: { newDropItem.localScale *= 1.5f; break; }
                }
                Debug.Log("Drop Item Value: " + maxValue + " by :" + transform.parent.name, transform.parent);
            }
                
            
            
        }
        
    }
    string GetTag()
    {
        return EnemyDropSpawner.Instance.GetDropItemForEnemy(enemyCtrl.EnemyTag);
    }

    
}
