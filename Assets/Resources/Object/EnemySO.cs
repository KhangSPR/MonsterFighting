using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets/Resources/Object/Enemy", menuName = "Object/Enemy", order = 1)]
public class EnemySO : ScriptableObject
{
    public int basePointsAttack;
    public int basePointsLife;
    public float basePointsAttackSpeed;
    public float basePointsSpecialAttack;
}
