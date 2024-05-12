using UnityEngine;
using UnityEngine.UI;

public abstract class BaseSlider : SaiMonoBehaviour
{
    [Header("Base Slider")]
    [SerializeField] protected Slider slider;
    protected override void Start()
    {
        base.Start();
        this.AddOnClickEnvent();
    }
    protected virtual void FixedUpdate()
    {
        //For Override
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSlider();
    }
    protected virtual void LoadSlider()
    {
        if (this.slider != null) return;
        this.slider = GetComponent<Slider>();
        Debug.LogWarning(transform.name + ": LoadSlider", gameObject);
    }
    protected virtual void AddOnClickEnvent()
    {
        this.slider.onValueChanged.AddListener(this.OnValueChanged);
    }
    protected abstract void OnValueChanged(float newValue);
}
