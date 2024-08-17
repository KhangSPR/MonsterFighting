using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyDeSpawn : DespawnByTime
{
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl => enemyCtrl;
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

        enemyCtrl.AbstractModel.EffectCharacter.SetMaterial(EffectManager.Instance.MaterialDefault);

        Transform newDropItem = EnemyDropSpawner.Instance.Spawn(GetTag(), transform.parent.position, Quaternion.identity);

        newDropItem.gameObject.SetActive(true);
    }
    string GetTag()
    {
        if (enemyCtrl == null) return "ItemGem";

        Debug.Log("GetTag = " + enemyCtrl.EnemyTag);

        return EnemyDropSpawner.Instance.GetDropItemForEnemy(enemyCtrl.EnemyTag);
    }
}
