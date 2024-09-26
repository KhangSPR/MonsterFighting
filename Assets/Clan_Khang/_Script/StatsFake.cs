using UnityEngine;

public class StatsFake : MonoBehaviour
{
    [SerializeField] private int _attack;
    [SerializeField] private int _life;
    [SerializeField] private float _attackSpeed;
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

    public float AttackSpeed
    {
        get => _attackSpeed;
        set => _attackSpeed = value;
    }

    public float CurrentMana
    {
        get => _currentMana;
        set => _currentMana = value;
    }

    public void Initialize(int attack, int life, float attackSpeed, float currentMana)
    {
        _attack = attack;
        _life = life;
        _attackSpeed = attackSpeed;
        _currentMana = currentMana;
    }
}
