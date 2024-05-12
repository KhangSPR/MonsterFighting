using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDameSender : DamageSender
{
    [SerializeField] protected BulletCtrl bulletCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadBulletCtrl();
    }
    protected virtual void loadBulletCtrl()
    {
        if (this.bulletCtrl != null) return;
        this.bulletCtrl = transform.parent.GetComponent<BulletCtrl>();
        Debug.Log(gameObject.name + ": loadDamageSender" + gameObject);
    }
    public override void Send(DamageReceiver receiver)
    {
        base.Send(receiver);
        Vector3 hitPos = transform.position;
        Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextDamageFX(hitPos);

        this.desTroyOBJ();
    }
    protected virtual void CreateTextDamageFX(Vector3 hitPos)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.SetDamage(this.dame);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return FXSpawner.textDamage;
    }
    public virtual void desTroyOBJ()
    {
        this.bulletCtrl.BulletDespawn.deSpawnObjParent();
    }
}