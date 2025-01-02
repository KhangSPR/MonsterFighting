using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBar : SaiMonoBehaviour
{
    [Header("HP Bar")]
    [SerializeField] protected ObjectCtrl objectCtrl;
    [SerializeField] protected Slider sliderHP;
    [SerializeField] protected Slider sliderMana;
    [SerializeField] protected FollowTarget followTarget;
    [SerializeField] protected Spawner spawner;
    protected virtual void FixedUpdate()
    {
        this.BarShowing();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadSliderHp();
        this.loadSliderMana();
        this.loadFollowTarget();
        this.loadSpawner();
    }
    protected virtual void loadSliderHp()
    {
        if (this.sliderHP != null) return;
        this.sliderHP = transform.Find("HPBar").GetComponentInChildren<Slider>();
        Debug.Log(gameObject.name + ": loadSliderHp" + gameObject);
    }
    protected virtual void loadSliderMana()
    {
        if (this.sliderMana != null) return;
        this.sliderMana = transform.Find("ManaBar").GetComponentInChildren<Slider>();
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
    protected virtual void BarShowing()
    {
        if(!this.objectCtrl.transform.gameObject.activeSelf)
        {
            BarSpawner.Instance.Despawn(transform);
        }
        if (this.objectCtrl == null) return;

        float hp = this.objectCtrl.ObjectDamageReceiver.IsHP;
        float maxHP = this.objectCtrl.ObjectDamageReceiver.IsMaxHP;
        float mana = this.objectCtrl.ObjMana.IsMana;
        float maxMana = this.objectCtrl.ObjMana.IsMaxMana;

        //Slider
        this.sliderHP.SetCurrentSlider(hp);
        this.sliderHP.SetMaxSlider(maxHP);
        this.sliderMana.SetCurrentSlider(mana);
        this.sliderMana.SetMaxSlider(maxMana);
        //CheckTarget IS Dead
        bool isDead = this.objectCtrl.AbstractModel.IsAnimationDeadComplete;
        if (isDead) this.spawner.Despawn(transform);
        //SET X,Y
        if (followTarget != null && followTarget.GetTarget() != null)
        {
            Transform target = followTarget.GetTarget();

            transform.position = target.position;
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
}
