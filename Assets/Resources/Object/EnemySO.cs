using System;
using UnityEngine;
public enum ItemDropType
{
    Crystalline,
    MagicalCrystal,
    WolfFangs,

}
[Serializable]
public class ItemDrop
{
    public ItemDropType itemDropType;
    //public 
    public int minDrop;
    public int maxDrop;
    // Ti le 1/10000 min 0.01 max 100
    [SerializeField, Range(0, 10000)] private float spawnRate;
    public float SpawnRate => spawnRate;
}
[CreateAssetMenu(fileName = "Assets/Resources/Object/Enemy", menuName = "Object/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public int basePointsAttack;
    public int basePointsLife;
    public float basePointsAttackSpeed; //Delay Attack
    public float basePointsCurrentMana;
    public float basePointsSpeedMove;
    public float attackSpeed;
    public float attackSpeedMelee;
    [Space]
    [Space]
    [Header("Item Drop")]
    public ItemDrop[] itemDrop;
    [Space]
    [Space]
    [Header("Skill")]
    public SkillSO skill1;
    public SkillSO skill2;

}
