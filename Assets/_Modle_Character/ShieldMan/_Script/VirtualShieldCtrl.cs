﻿using DG.Tweening; // Đảm bảo đã import DoTween
using System.Collections.Generic;
using UnityEngine;

public class VirtualShieldCtrl : SkillCtrl, ITrapHpSkill
{
    public float trapHp;
    public float TrapHp => trapHp;

    public float moveRange = 0.2f; // Khoảng cách di chuyển
    public float moveSpeed = 1f;  // Tốc độ di chuyển

    private Tween moveTween; // Biến lưu tween

    public List<EnemyCtrl> enemyCtrls;

    protected override void OnEnable()
    {
        base.OnEnable();
        SkillAction();
        // Event
        this.fxDespawn.OnFXSkill += Trigger;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        StopEffect();

        // Hủy event
        this.fxDespawn.OnFXSkill -= Trigger;

        // Dừng hiệu ứng di chuyển
        StopMoveUpDown();
    }

    public override void SkillAction()
    {
        // Logic cho skill
        MoveUpDown();
    }

    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        // Logic xử lý va chạm
    }

    private void MoveUpDown()
    {
        // Di chuyển lên và xuống nhẹ nhàng, tạo hiệu ứng mượt mà
        moveTween = transform.DOMoveY(transform.position.y + moveRange, moveSpeed)
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại vô hạn, kiểu Yoyo (di chuyển lên rồi xuống)
            .SetEase(Ease.InOutSine) // Dùng easing InOutSine cho chuyển động nhẹ nhàng
            .OnKill(() => Debug.Log("Animation stopped")); // Callback khi tween bị kill
    }

    private void StopMoveUpDown()
    {
        // Dừng tween hiện tại nếu đang chạy
        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill(); // Dừng tween
            moveTween = null;
        }
    }

    // Skill Virtual Shield
    [SerializeField] FragmentFadingArray fragmentFadingArray;
    public void Trigger()
    {
        fragmentFadingArray.TriggerFading();
        OnSkillCompleteSkill();
    }

    protected void OnSkillCompleteSkill()
    {
        this.objectCtrl.AbstractModel.OnSkillEnabeleComplete();
        this.objectCtrl.AbstractModel.IsStun = false;


        Debug.Log("OnSkillCompleteSkill");
    }
    private void StopEffect()
    {
        foreach (EnemyCtrl enemyCtrl in enemyCtrls)
        {
            RemoveMagicVortexCtrl(enemyCtrl.TargetSkillScript.listSkillCtrl);

            if (enemyCtrl.EnemyAttack.ListObjAttacks.Count <= 0 &&
                enemyCtrl.TargetSkillScript.listSkillCtrl.Count <= 0)
            {
                enemyCtrl.EnemyAttack.CheckCanAttack = false;
            }
        }
    }

    public void RemoveMagicVortexCtrl(List<SkillCtrl> listSkillCtrl)
    {
        // Duyệt qua danh sách để tìm instance của MagicVortexCtrl
        for (int i = 0; i < listSkillCtrl.Count; i++)
        {
            if (listSkillCtrl[i] is VirtualShieldCtrl virtualShieldCtrl && this == virtualShieldCtrl)
            {
                listSkillCtrl.RemoveAt(i); // Xóa phần tử tại vị trí tìm được


                return; // Thoát khỏi hàm sau khi xóa
            }
        }
    }
}
