using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjModleAbstact : SaiMonoBehaviour
{
    [SerializeField] protected AbstractModel abstractModel;
    public AbstractModel AbstractModel => abstractModel;
     
    [SerializeField] protected EnemyCtrl enemyCtrl;
    public EnemyCtrl EnemyCtrl => enemyCtrl;
    [SerializeField] protected PlayerCtrl playerCtrl;
    public PlayerCtrl PlayerCtrl => playerCtrl;
    [SerializeField] protected SkillCtrl skillCtrl;
    public SkillCtrl SkillCtrl => skillCtrl;
    [SerializeField] protected ObjectCtrl objectCtrl;
    public ObjectCtrl ObjectCtrl => objectCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAbstractModel();
        this.LoadSkillCtrl();
        this.LoadObjCtrl();
    }
        
    protected void LoadAbstractModel()
    {
        if (abstractModel != null) return;

        this.abstractModel = transform.parent.GetComponent<AbstractModel>();
    }
    protected void LoadObjCtrl()
    {
        if (transform.parent.name == "Castle") return;
        if (skillCtrl != null) return;

        this.objectCtrl = this.abstractModel.ObjectCtrl;

        if (this.abstractModel.EnemyCtrl != null)
        {
            enemyCtrl = this.abstractModel.EnemyCtrl;
        }
        else 
        {
            playerCtrl = this.abstractModel.PlayerCtrl;
        }

        Debug.Log("LoadObjCtrl");
    }
    protected void LoadSkillCtrl()
    {
        if (skillCtrl != null) return;
        
        this.skillCtrl = transform.parent.GetComponent<SkillCtrl>();
    }
}
