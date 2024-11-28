using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : BaseSlider
{
    [Header("Slider HP")]
    [SerializeField] protected float maxHP = 100;
    [SerializeField] protected float currentHP = 70;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.HPShowing();
    }
    protected override void OnValueChanged(float newValue)
    {
        //
    }
    public virtual void SetMaxHp(float maxHP)
    {
        this.maxHP = maxHP;
    }
    public virtual void SetCurrentHP(float currentHP)
    {
        this.currentHP = currentHP;
    }
    protected virtual void HPShowing()
    {
        float hpPercent = this.currentHP / this.maxHP;
        this.slider.value = hpPercent;
    }
}
