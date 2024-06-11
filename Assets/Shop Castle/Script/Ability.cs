using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ability/DefaultAbility", fileName = "DefaultAbility")]
public class Ability : ScriptableObject
{
    [Header("private variable")]
    [SerializeField, ReadOnlyInspector]
    protected int money_PlusDefault = 25;  //5 Money Default
    [SerializeField, ReadOnlyInspector]
    protected int hp_PlusDefault = 50; //2 Hp Default
    [SerializeField, ReadOnlyInspector]
    protected int gold_money_PlusDefault = 1; //Increase 1$ / 5s
    [SerializeField, ReadOnlyInspector]
    protected int slot_PlusDefault = 3; //3 slot Card Play
    public virtual void Active(GameObject parent )
    {
        GameManager.Instance.CostManager.Currency = money_PlusDefault;
        GameManager.Instance.max_hp = hp_PlusDefault;

    }
    public virtual void Active()
    {

    }
}
