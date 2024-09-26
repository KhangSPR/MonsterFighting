using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaHandler : AbstractCtrl
{
    [Header("Mana Handler")]
    [SerializeField] protected float isMana = 1;
    public float IsMana => isMana;
    [SerializeField] protected float isMaxMana = 3;
    public float IsMaxMana => isMaxMana;
    protected override void OnEnable()
    {
        ReBorn();
    }
    public virtual void ReBorn()
    {
        isMana = isMaxMana;
    }

    protected virtual void AddMana(int amount)
    {
        isMana = Mathf.Min(isMana + amount, isMaxMana);
    }

    public virtual void DeductMana(float amount)
    {
        isMana -= amount;
    }
}
