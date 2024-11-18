// Object damage receiver class
using UnityEngine;

public class ObjectDamageReceiver : DamageReceiverByType
{
    public override void OnDead()
    {
        base.OnDead();

        //objectCtrl.Despawn.ResetCanDespawnFlag(); //In Animation Dead ObjModle
        this.AbstractModel.DameFlash.StopCoroutieSlash();
        if (this.enemyCtrl)
        {
            if (enemyCtrl.TargetSkillScript.listSkillCtrl.Count <= 0) return;

            foreach (SkillCtrl scrSkill in enemyCtrl.TargetSkillScript.listSkillCtrl)
            {
                scrSkill.FxDespawn.ResetCanDespawnFlag();
            }
        }

    }
    public override void ReBorn()
    {
        this.LoadObjCtrl();
        isMaxHP = PlayerCtrl?.CharacterStatsFake.Life
                  ?? EnemyCtrl?.EnemySO.basePointsLife
                  ?? isMaxHP;

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
        string damageNumber = LargeNumber.ToString(point);;

        string fxName = "TextDamage";
        Transform fxObj = FXSpawner.Instance.Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(damageNumber, medicineType);
        fxObj.gameObject.SetActive(true);
    }
    protected void AddPointType(int amount, Medicine medicineType)
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