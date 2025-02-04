using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjMana : ManaHandler
{
    public override void ReBorn()
    {
        // Ensure the ObjectCtrl and its dependencies are loaded first

        if (objCtrl != null)
        {
            if (PlayerCtrl != null && PlayerCtrl.CardCharacter != null && PlayerCtrl.CardCharacter.CharacterStats != null)
            {
                isMaxMana = PlayerCtrl.CardCharacter.CharacterStats.Mana;
            }
            else if (EnemyCtrl != null && EnemyCtrl.EnemySO != null)
            {
                isMaxMana = EnemyCtrl.EnemySO.basePointsCurrentMana;
            }

            Debug.Log("ReBorn Mana = " + isMaxMana);
        }

        Debug.Log("Debug Reborn Mana");


        base.ReBorn();
    }
}
