using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRainHit : MonoBehaviour, IElectricable
{
    [SerializeField] DamageSender damageSender;
    public DamageSender DamageSender => damageSender;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent == null) return;

        if (collision.name != "Modle") return;

        if (collision.transform.parent.CompareTag("Enemy"))
        {
            damageSender.Send(collision.transform.parent);
            ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();          
            DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

            if (damageReceiver == null)
            {
                Debug.Log("Null: DamageReceiver");
                return;
            }

            if (damageReceiver.IsDead) return;


            //Add Skill
            objectCtrl.ObjectDamageReceiver.StartStun();
        }
    }
    #region FX_StartBurning_Coroutine
    public void StartTwitching(int damagePerSecond)
    {
    }

    public void StopTwitching()
    {
    }
    #endregion
}
