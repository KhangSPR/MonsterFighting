using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VenomousExplosionSphereCtrl : SkillCtrl
{
    [SerializeField]
    private float timer;
    [SerializeField]
    private bool stopActionSkill;
    [SerializeField]
    private float timeDuration;

    protected override void OnEnable()
    {
        base.OnEnable();
        // Reset timer và đánh dấu chưa gọi hàm khi đối tượng được tái sử dụng
        timer = 0f;
        stopActionSkill = false;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        this.fXImpact.ClearCollider();

    }
    public override void SkillAction()
    {
    }

    protected override void Update()
    {
        base.Update();
        if (this.stopActionSkill) return;
        timer += Time.deltaTime;
        if (timer >= timeDuration) stopActionSkill = true;
    }
    public override void SkillColliderCastle(CastleCtrl castleCtrl)
    {
        base.SkillColliderCastle(castleCtrl);
        DamageReceiver damageReceiver = castleCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }

        if (damageReceiver.IsDead) return;

        this.DamageSender.Send(damageReceiver);

    }
    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }
        objectCtrl.ObjectDamageReceiver.StartPotioning(this.damageSender.SkillType);

        if (damageReceiver.IsDead) return;

        // Kiểm tra nếu thời gian chưa vượt quá 1 giây và hiệu ứng chưa được kích hoạt
        if (!stopActionSkill)
        {
            // Gọi hiệu ứng stun và impact
            objectCtrl.ObjectDamageReceiver.StartStun();
            this.DamageSender.SendFXImpact(damageReceiver, objectCtrl);

        }
        else
        {
            // Nếu vượt quá 1 giây, không làm gì thêm
            Debug.Log("Timer exceeded 1 second, effects not triggered.");
        }
    }
    private void MoveVenomousExplosionSphere()
    {

    }
}
