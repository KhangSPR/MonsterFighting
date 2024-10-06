// Object damage receiver class
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class ObjectDamageReceiver : DamageReceiverByType
{
    public override void OnDead()
    {
        base.OnDead();
        if (objectCtrl != null)
        {
            //objectCtrl.Despawn.ResetCanDespawnFlag(); //In Animation Dead ObjModle
            objectCtrl.AbstractModel.DameFlash.StopCoroutieSlash();
            if(enemyCtrl !=null)
            {
                if (enemyCtrl.TargetSkill.listSkillCtrl.Count <= 0) return;

                foreach(SkillCtrl scrSkill in enemyCtrl.TargetSkill.listSkillCtrl)
                {
                    scrSkill.FxDespawn.ResetCanDespawnFlag();
                }
            }
        }
        else
        {
            Debug.LogError("ObjectCtrl is not assigned for " + gameObject.name);
        }
    }  
    public override void ReBorn()
    {
        // Ensure the ObjectCtrl and its dependencies are loaded first
        LoadObjectCtrl();

        if (objectCtrl != null)
        {
            isMaxHP = PlayerCtrl?.CharacterStatsFake.Life
                      ?? EnemyCtrl?.EnemySO.basePointsLife
                      ?? isMaxHP;
        }

        base.ReBorn();
    }
    public void AddPoint(int amount, Medicine medicineType)
    {
        this.AddPointType(amount, medicineType);    
        this.AddPointText(amount, medicineType);
    }
    #region FX Text ... 
    protected void AddPointText(int point, Medicine medicineType)
    {
        Vector3 hitPos = transform.position;
        hitPos.y += 0.5f;
        Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextPointFX(point, hitPos, medicineType);
    }
    protected virtual void CreateTextPointFX(int point, Vector3 hitPos, Medicine medicineType)
    {
        string fxName = "TextDamage";
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(point, medicineType);
        fxObj.gameObject.SetActive(true);
    }
    protected void AddPointType(int amount,Medicine medicineType)
    {
        switch (medicineType)
        {
            case Medicine.Heal:
                this.AddHealth(amount);
                break;

            case Medicine.Power:
                //this.playerCtrl.DamageSender.AddPower(amount);
                break;

            // Các trường hợp khác nếu có

            default:
                Debug.LogWarning("Unknown medicine type: " + medicineType);
                break;
        }
    }
    #endregion
}