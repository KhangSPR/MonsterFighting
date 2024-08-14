using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : SaiMonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] protected ObjectCtrl objectCtrl;
    [SerializeField] protected SliderHp sliderHP;
    [SerializeField] protected FollowTarget followTarget;
    [SerializeField] protected Spawner spawner;
    protected virtual void FixedUpdate()
    {
        this.HPShowing();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadSliderHp();
        this.loadFollowTarget();
        this.loadSpawner();
    }
    protected virtual void loadSliderHp()
    {
        if (this.sliderHP != null) return;
        this.sliderHP = transform.GetComponentInChildren<SliderHp>();
        Debug.Log(gameObject.name + ": loadSliderHp" + gameObject);
    }
    protected virtual void loadSpawner()
    {
        if (this.spawner != null) return;
        this.spawner = transform.parent.parent.GetComponent<Spawner>();
        Debug.Log(gameObject.name + ": loadSpawner" + gameObject);
    }
    protected virtual void loadFollowTarget()
    {
        if (this.followTarget != null) return;
        this.followTarget = transform.GetComponentInChildren<FollowTarget>();
        Debug.Log(gameObject.name + ": loadFollowTarget" + gameObject);
    }
    protected virtual void HPShowing()
    {
        float hp = this.objectCtrl.ObjectDamageReceiver.IsHP;
        float maxHP = this.objectCtrl.ObjectDamageReceiver.IsMaxHP;
        
        this.sliderHP.SetCurrentHP(hp);
        this.sliderHP.SetMaxHp(maxHP);
        //CheckTarget IS Dead
        bool isDead = this.objectCtrl.ObjectDamageReceiver.IsDead;
        if (isDead) this.spawner.Despawn(transform);
        //SET X,Y
        if (followTarget != null && followTarget.GetTarget() != null)
        {
            Transform target = followTarget.GetTarget();

            // Sử dụng switch case để xử lý từng trường hợp
            switch (target.name)
            {
                case "Enemy_1":
                    SetSliderPosition(0,0.7f);
                    break;
                case "Enemy_2":
                    SetSliderPosition(0,1.1f);
                    break;
                case "Enemy_3":
                    SetSliderPosition(-0.2f, 0.8f);
                    break;
                // Thêm các trường hợp khác ở đây nếu cần
                default:
                    // Mặc định, nếu không phải là bất kỳ enemy nào
                    SetSliderPosition(0f,0f); // Đặt lại vị trí slider về 0
                    break;
            }
        }
    }
    public virtual void SetObjectCtrl(ObjectCtrl objectCtrl)
    {
        this.objectCtrl = objectCtrl;
    }
    public virtual void SetFollowTarget(Transform target)
    {
        this.followTarget.SetTarget(target);
    }
    // Hàm để thiết lập vị trí y của slider
    protected void SetSliderPosition(float xPos, float yPos)
    {
        Vector3 sliderPosition = this.sliderHP.transform.localPosition;
        sliderPosition.x = xPos;
        sliderPosition.y = yPos;
        this.sliderHP.transform.localPosition = sliderPosition;
    }

}
