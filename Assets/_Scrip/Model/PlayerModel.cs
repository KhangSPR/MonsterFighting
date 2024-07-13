using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerModel : AbstractModel
{
    public Transform bullet;
    public Transform InfernoSphere_prefab;
    public Transform bulletSpawnPosition;
    

    public bool check = true;
    protected override void AnimationLoading()
    {
        bool shouldAttack = false;

        switch (currentState)
        {
            case State.Idle:
                PlayAnimation("Attack", false);
                isAttacking = false;
                break;
            case State.Attack:
                var isMana = transform.parent.GetComponent<PlayerCtrl>().mana;
                var skillLevelCurrent = transform.parent.GetComponent<PlayerCtrl>().SkillLevel;
                var manaNeed = 0;
                switch (skillLevelCurrent)
                {
                    case 0: manaNeed = 0;break;
                    case 1: manaNeed = transform.parent.GetComponent<PlayerCtrl>().skill1ManaNeed; break;
                    case 2: manaNeed = transform.parent.GetComponent<PlayerCtrl>().skill2ManaNeed; break;
                    case 3: manaNeed = transform.parent.GetComponent<PlayerCtrl>().skill3ManaNeed; break;
                }

                Debug.Log("!isAttacking && currentDelay <= 0 && transform.parent.GetComponent<PlayerCtrl>().mana >= manaNeed:" + (!isAttacking && currentDelay <= 0 && transform.parent.GetComponent<PlayerCtrl>().mana >= manaNeed));
                if ((!isAttacking && currentDelay <= 0 && transform.parent.GetComponent<PlayerCtrl>().mana >= manaNeed)==true)
                {
                    try
                    {
                        PlayAnimation("Skill Level", skillLevelCurrent);
                        transform.parent.GetComponent<PlayerCtrl>().UpdateManabar(isMana);
                        transform.parent.GetComponent<PlayerCtrl>().mana -= manaNeed;
                        transform.parent.GetComponent<PlayerCtrl>().UpdateManabar(transform.parent.GetComponent<PlayerCtrl>().mana);

                        isAttacking = true;
                        PlayAnimation("Attack", isAttacking);
                        //isAnimationComplete = false;
                    }
                    catch{
                        Debug.LogError("Skill này không có hoặc chưa được học");
                    }
                }
                break;
        }

        if (this.playerCtrl.PlayerAttack.canAttack)
        {
            shouldAttack = true;
        }

        if (shouldAttack)
        {
            currentState = State.Attack;
        }
        else
        {
            currentState = State.Idle;
        }

        if (isAttacking && isAnimationComplete)
        {
            Debug.Log("AttackType");

            this.AttackType();
            isAnimationComplete = false;
            currentDelay = delayAttack; // Set time Wait

            Debug.Log(isAnimationComplete);

        }
    }
    public void SetStateIdle()
    {
        currentState = State.Idle; // Next State Idle

    }
    protected override void AttackType()
    {
        switch (attackType)
        {
            case attackType.BulletDefault:
                this.playerCtrl.PlayerShooter.Shoot();
                break;
            case attackType.BulletPX:
                this.playerCtrl.PlayerShooter.ShootPX();
                break;
            case attackType.Animation:
                Debug.Log("animationImpact");

                animationImpact.damageSent = false;
                animationImpact.gameObject.SetActive(true);
                break;
            default:
                // Xử lý cho các trường hợp khác (nếu cần)
                break;
        }
    }

    public void SpawnBullet()
    {
        Transform bullet = Instantiate(this.bullet, bulletSpawnPosition.position, Quaternion.identity, transform.parent);
        Destroy(bullet.gameObject, 10f);
    }
    public void SpawnInfernoSphere()
    {
        var Enemies= Physics2D.RaycastAll(transform.position, Vector2.right, float.MaxValue);
        var NearestEnemy = new RaycastHit2D();
        if (Enemies.Length >0)
        {
            foreach (var enemy in Enemies) { 
                if(enemy.transform.parent.tag == "Enemy")
                {
                    NearestEnemy = enemy;break;
                }
            }
        }
        Transform inferno_sphere = Instantiate(this.InfernoSphere_prefab, NearestEnemy.point, Quaternion.identity, transform.parent);
    }

}
