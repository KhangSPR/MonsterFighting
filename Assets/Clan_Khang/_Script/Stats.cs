
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Card/Create New Stats")]
public class Stats : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] private int _attack;
    [SerializeField] private int _life;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _attackSpeedMelee;
    [SerializeField] private float _currentMana;
    // Public properties with getter and setter
    public int Attack
    {
        get => _attack;
        set => _attack = value;
    }

    public int Life
    {
        get => _life;
        set => _life = value;
    }
    public float AttackSpeedMelee
    {
        get => _attackSpeedMelee;
        set => _attackSpeedMelee = value;
    }

    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float CurrentManaAttack
    {
        get => _currentMana;
        set => _currentMana = value;
    }


    public Stats(int attack, int life, float attackSpeed, float currentMana, float attackSpeedMelee)
    {
        _attack = attack;
        _life = life;
        _attackSpeed = attackSpeed;
        _currentMana = currentMana;
        _attackSpeedMelee = attackSpeedMelee;
    }
}
