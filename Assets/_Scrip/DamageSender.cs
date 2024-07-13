using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class DamageSender : SaiMonoBehaviour
{
    //public float damageInterval = 3f;
    public int dame;
    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) return;
        this.Send(damageReceiver);
        //this.createImpactFX();
    }
    public virtual void Send(Transform obj, DamageSender damageSender)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) return;
        this.Send(damageReceiver, damageSender);
        //this.createImpactFX();
    }

    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.deDuctHP(this.dame);
        var animator = damageReceiver.transform.parent.GetComponentInChildren<Animator>();
        animator.SetTrigger("Hit");
        Debug.Log("Send" + this.dame);
    }

    public virtual void Send(DamageReceiver damageReceiver,DamageSender damageSender)
    {
        var tag = damageSender.transform.parent.tag;
        var name = damageSender.transform.parent.name;
        Debug.Log(" tag:"+ tag+" ,name :" +name,damageSender.gameObject);
        if(tag == "Player" || name.Contains("Bullet_"))
        {
            damageReceiver.deDuctHP(this.dame,true);
        }
        else
        {
            damageReceiver.deDuctHP(this.dame);
        }

        var animator = damageReceiver.transform.parent.GetComponentInChildren<Animator>();
        animator.SetTrigger("Hit");

        Debug.Log("Send " + this.dame);
    }

    //public void SendDamageOverTime(Transform obj, bool isColliding)
    //{
    //    DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
    //    if (damageReceiver == null) return;

    //    if (isColliding)
    //    {
    //        // Đặt lại damageInterval về ban đầu
    //        damageInterval = 3f;
    //        StartCoroutine(SendDamageOverTimeCoroutine(damageReceiver));
    //    }
    //    else
    //    {
    //        // Nếu không còn va chạm, bắt đầu gửi dame theo thời gian
    //        StartCoroutine(SendDamageOverTimeCoroutine(damageReceiver));
    //    }
    //}

    //private IEnumerator SendDamageOverTimeCoroutine(DamageReceiver damageReceiver)
    //{
    //    while (damageInterval > 0f)
    //    {
    //        yield return new WaitForSeconds(1f); // Chờ 0.3s trước khi gửi tiếp
    //        this.Send(damageReceiver); // Gửi dame
    //        damageInterval -= 0.5f; // Giảm thời gian đợi

    //    }

    //    // Đảm bảo damageInterval không nhỏ hơn 0
    //    damageInterval = Mathf.Max(damageInterval, 0f);
    //}
}









//protected virtual void createImpactFX()
//{
//    string fxName = this.getImpactFX();
//    Vector3 hitPos = transform.position;
//    Quaternion hitRot = transform.rotation;
//    Transform fxImpact = FXSpawner.Instance.Spawn(fxName, hitPos, hitRot);
//    fxImpact.gameObject.SetActive(true);
//}
//protected virtual string getImpactFX()
//{
//    return FXSpawner.ImpactOne;
//}


