using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardModel : GuardAbstract
{
    [SerializeField] protected Animator animator;
    [SerializeField] bool canAttack = false;

    bool isAttacking = false; // Đánh dấu animation "Attack" đang chạy hay không
    bool isAnimationComplete = false; // Đánh dấu xem animation đã hoàn thành hay chưa
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAnimator();
    }
    protected void LoadAnimator()
    {
        if (animator != null) return;
        this.animator = transform.GetComponent<Animator>();
        Debug.Log(transform.name + ": LoadLoadAnimator", gameObject);
    }
    private void FixedUpdate()
    {
        AnimationLoading();
    }

    private void AnimationLoading()
    {
        canAttack = this.guardCtrl.GuardShooter.isShooting;

        if (canAttack)
        {
            if (!isAttacking)
            {
                // Nếu animation "Attack" không đang chạy, bắt đầu nó
                animator.SetBool("Attack", true);
                isAttacking = true;
                isAnimationComplete = false;
            }
        }
        else
        {
            // Nếu không thể tấn công, tắt animation "Attack"
            animator.SetBool("Attack", false);
            isAttacking = false;
            Idle();
        }

        if (isAttacking && isAnimationComplete)
        {
            // Animation đã hoàn thành, thực hiện spawn đạn tại đây
            this.guardCtrl.GuardShooter.Shoot();
            isAnimationComplete = false; // Đặt lại biến cho lần tiếp theo
        }
    }

    // Hàm này sẽ được gọi từ Animation Event khi animation kết thúc
    public void OnAttackAnimationEnd()
    {
        isAnimationComplete = true;
    }

    private void Idle()
    {
        animator.Play("Idle");
    }
}
