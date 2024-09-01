using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class KillEnemyCondition : StarsCondition
{
    public uint enemyKill;

    public override void SetDefaultValue()
    {
        enemyKill = 0;
        currentThreshold = enemyKill;
    }

    public override uint CheckThreshold()
    {

        uint result = 0;
        uint enemyKill = this.enemyKill;
        if (enemyKill >= threshold1) result = 1;
        if(enemyKill >= threshold2) result = 2;
        if(enemyKill >= threshold3) result = 3;
        Debug.Log($"Số lượng enemy giết được : {enemyKill} , số sao đạt được : {result}");
        return result;
    }
}
