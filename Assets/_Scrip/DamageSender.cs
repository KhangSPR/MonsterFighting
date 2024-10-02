using System.Collections;
using UnityEngine;

public class DamageSender : AbstractCtrl
{
    [SerializeField]
    int damage;
    public int Damage { get { return damage; } set { damage = value; } }

    [SerializeField] protected SkillType skillType;
    public SkillType SkillType => skillType;  // Sửa lỗi ở đây

    protected override void Start()
    {
        base.Start();

        this.damage = playerCtrl?.CharacterStatsFake.Attack ?? enemyCtrl?.EnemySO.basePointsAttack ?? this.damage;
    }
    public virtual void AddPower(int amount)
    {
        if (playerCtrl.ObjectDamageReceiver.IsDead) return;
        damage += amount;
    }
    public virtual void Send(Transform obj)
    {
        DamageReceiver damageReceiver = obj.GetComponentInChildren<DamageReceiver>();
        if (damageReceiver == null) return;
        this.Send(damageReceiver);

        FXSpawner.Instance.SendFXText(damage, skillType, obj, Quaternion.identity);

        Debug.Log("Default Spawner Sender");
    }
    public virtual void SendFXImpact(DamageReceiver damageReceiver)
    {
        //DamageReceiver damageReceiver = obj.GetComponent<DamageReceiver>();

        this.Send(damageReceiver);

        FXSpawner.Instance.SendFXText(damage, skillType, damageReceiver.transform.parent, Quaternion.identity);

        Debug.Log("FX Spawner Sender");

    }
    public virtual void Send(DamageReceiver damageReceiver)
    {
        damageReceiver.DeductHealth(this.damage);

        Debug.Log("Send" + this.damage);
    }
    #region Comment
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
#endregion

