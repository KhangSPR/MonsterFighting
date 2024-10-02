using UnityEngine;
using DG.Tweening;

public class BallOfDarknessCtrl : SkillCtrl, IDarkable
{
    public Transform targetPosition; 
    public float moveDuration = 2f;

    public override void SkillAction()
    {

        MoveBallOfDarkness();
    }

    public override void SkillColider(ObjectCtrl objectCtrl)
    {
        DamageReceiver damageReceiver = objectCtrl.GetComponentInChildren<DamageReceiver>();

        if (damageReceiver == null)
        {
            Debug.Log("Null: DamageReceiver");
            return;
        }

        if (damageReceiver.IsDead) return;


        //Add Skill
        this.DamageSender.SendFXImpact(damageReceiver);


        //Debug.Log("Call Skill Colider");
    }

    private void MoveBallOfDarkness()
    {
        transform.DOMove(targetPosition.position, moveDuration)
            .SetEase(Ease.InOutQuad)  
            .OnComplete(() =>
            {
                Debug.Log("Ball of Darkness reached target position.");
            });
    }
    #region FX_StartDarking_Coroutine
    public void StartDarking(int damagePerSecond)
    {
        throw new System.NotImplementedException();
    }

    public void StopDarking()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
