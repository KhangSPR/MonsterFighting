using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Stars Condition/Kill Enemy")]
public class KillEnemyCondition : StarsCondition
{
    public int enemyKill;

    public override void SetDefaultValue()
    {
        enemyKill = 0;
        currentThreshold = enemyKill;
    }

    public override float CheckThreshold()
    {
        
        float result = 0;
        float enemyKill = (float)this.enemyKill;
        if (enemyKill >= threshold1) result = 1;
        if(enemyKill >= threshold2) result = 2;
        if(enemyKill >= threshold3) result = 3;
        Debug.Log($"Số lượng enemy giết được : {enemyKill} , số sao đạt được : {result}");
        return result;
    }
}
