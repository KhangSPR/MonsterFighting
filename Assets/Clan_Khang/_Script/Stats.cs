
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Card/Create New Stats")]
public class Stats : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] private int _attack;
    [SerializeField] private int _life;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _specialAttack;
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

    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float SpecialAttack
    {
        get => _specialAttack;
        set => _specialAttack = value;
    }


    public Stats(int attack, int life, float attackSpeed, float specialAttack)
    {
        _attack = attack;
        _life = life;
        _attackSpeed = attackSpeed;
        _specialAttack = specialAttack;
    }
}
