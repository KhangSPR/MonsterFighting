using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : EnemyAbstract
{
    [SerializeField] private bool isSkill = false;
    public bool IsSkill { get { return isSkill; } set { isSkill = value; } }
}
