using DG.Tweening;
using UnityEngine;

public class FireSlashCtrl : SkillCtrl, IBurnable
{
    [SerializeField] ParticleSystem _particleSystem;
    public Transform targetPosition; // Vị trí mục tiêu
    public float moveDuration = 1.4f;  // Thời gian di chuyển
    [ContextMenu("Skill Fire")]
    public override void SkillAction()
    {
        this.MoveFireDual();
    }

    private void MoveFireDual()
    {
        // Kiểm tra và fade màu của ParticleSystem
        if (_particleSystem != null)
        {
            var mainModule = _particleSystem.main;
            // Đặt màu khởi đầu của hạt là màu trắng với độ mờ là 1
            mainModule.startColor = new Color(1, 1, 1, 1);

            // Thực hiện fade màu từ màu trắng có độ mờ 1 sang trong suốt (0) trong suốt thời gian di chuyển
            DOTween.To(() => mainModule.startColor.color, x => mainModule.startColor = new ParticleSystem.MinMaxGradient(x), new Color(1, 1, 1, 0f), 0.6f);
        }

        // Di chuyển object đến vị trí đích
        transform.DOMove(targetPosition.position, moveDuration)
            .OnComplete(() =>
            {
                FXSpawner.Instance.Despawn(transform); // Đưa đối tượng về Pool
            });
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
        objectCtrl.ObjectDamageReceiver.StartStun();
        this.DamageSender.SendFXImpact(damageReceiver, objectCtrl);
    }
    #region FX_StartBurning_Coroutine
    public void StartBurning(int damagePerSecond)
    {
        throw new System.NotImplementedException();
    }

    public void StopBurning()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
