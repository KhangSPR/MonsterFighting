using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Debuff/Burn")]
public class Burn : Debuff
{
    public float burnInterval; //Tick
    public float burnTotalTime;
    public int burnDamage;
    private void Awake()
    {
        //ApplyDebuff();
    }

    public override void ApplyDebuff(Transform target)
    {
        CoroutineRunner.instance.StartCoroutine(Burning(target));
    }

    IEnumerator Burning(Transform target)
    {
        float burnTime = burnTotalTime;
        while(burnTime > 0)
        {
            Debug.Log("Burn Time : " + burnTime,target.Find("DamageReceiver"));
            Debug.Log("Target HP:" + target.Find("DamageReceiver").GetComponent<ObjectDamageReceiver>().isHP);
            yield return new WaitForSeconds(burnInterval);
            burnTime -= burnInterval;
            target.Find("DamageReceiver").GetComponent<ObjectDamageReceiver>().deDuctHP(burnDamage);
        }
    }
}
