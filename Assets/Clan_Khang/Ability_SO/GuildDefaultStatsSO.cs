using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GuildDefaultStats", menuName = "UIGuild/DefaultStats", order = 1)]
public class GuildDefaultStatsSO : ScriptableObject
{
    [Header("Default Stats")]
    [SerializeField, ReadOnlyInspector]
    private int defaultMoney = 5; // 5 Money Default
    public int DefaultMoney => defaultMoney;

    [SerializeField, ReadOnlyInspector]
    private int defaultHp = 1000; // 1000 Hp Default
    public int DefaultHp => defaultHp;

    [SerializeField, ReadOnlyInspector]
    private int defaultGoldIncrease = 1; // Increase $1 / 5s
    public int DefaultGoldIncrease => defaultGoldIncrease;

    [SerializeField, ReadOnlyInspector]
    private int defaultSlot = 7; // 7 slot Card Play
    public int DefaultSlot => defaultSlot;

    public virtual void ApplyDefaultStats(GameObject parent)
    {
        GameManager.Instance.CostManager.Currency = defaultMoney;
        GameManager.Instance.max_hp = defaultHp;
        GameManager.Instance.CostManager.SetAutoIncreaseAmount(defaultGoldIncrease);
    }
    public virtual void ApplyMoreStats(GameObject parent) { }
}
