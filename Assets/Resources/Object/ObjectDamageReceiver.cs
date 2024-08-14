// Object damage receiver class
using UnityEngine;

public class ObjectDamageReceiver : DamageReceiverByType
{
    [Header("Object")]
    [SerializeField] protected ObjectCtrl ObjectCtrl;

    protected override void LoadValue()
    {
        base.LoadValue();
        LoadObjectCtrl();
    }

    protected virtual void LoadObjectCtrl()
    {
        if (ObjectCtrl != null) return;
        ObjectCtrl = transform.parent.GetComponent<ObjectCtrl>();
        Debug.Log(gameObject.name + ": Loaded ObjectCtrl for " + gameObject.name);
    }

    public override void OnDead()
    {
        if (ObjectCtrl != null)
        {
            ObjectCtrl.Despawn.ResetCanDespawnFlag();
            ObjectCtrl.AbstractModel.DameFlash.StopCoroutieSlash();
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

        if (ObjectCtrl != null)
        {
            isMaxHP = PlayerCtrl?.CardCharacter.CharacterStats.Life
                      ?? EnemyCtrl?.EnemySO.basePointsLife
                      ?? isMaxHP;
        }

        base.ReBorn();
    }
    protected override void DameSlash()
    {
        base.DameSlash();
        ObjectCtrl.AbstractModel.DameFlash.CallDamageFlash();
    }
}