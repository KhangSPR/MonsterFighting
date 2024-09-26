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
            isMaxMana = PlayerCtrl?.CardCharacter.CharacterStats.CurrentManaAttack
                      ?? EnemyCtrl?.EnemySO.basePointsCurrentMana
                      ?? isMaxMana;

            Debug.Log("ReBorn Mana = " + isMaxMana);
        }

        Debug.Log("Debug Reborn Mana");


        base.ReBorn();
    }
}
