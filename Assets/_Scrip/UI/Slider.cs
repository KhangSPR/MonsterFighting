using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : BaseSlider
{
    [Header("Slider HP")]
    [SerializeField] protected float maxSlider = 100;
    public float MaxSlider => maxSlider;
    [SerializeField] protected float currentSlider = 70;
    public float CurrentSlider => currentSlider;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.HPShowing();
    }
    protected override void OnValueChanged(float newValue)
    {
        //
    }
    public virtual void SetMaxSlider(float maxHP)
    {
        this.maxSlider = maxHP;
    }
    public virtual void SetCurrentSlider(float currentHP)
    {
        this.currentSlider = currentHP;
    }
    protected virtual void HPShowing()
    {
        float hpPercent = this.currentSlider / this.maxSlider;
        this.slider.value = hpPercent;
    }
}
