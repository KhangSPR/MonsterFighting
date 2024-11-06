using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public class WizardStatIncreaseStrategy : _IStatIncreaseStrategy
{
    private float increasePercentage = 0.30f; // Tăng 30% sức mạnh phép thuật


    public void IncreaseStats(GameObject parent, AttackCategory attackType)
    {
        if (parent != null)
        {
            PlayerCtrl playerCtrl = parent.GetComponent<PlayerCtrl>();
            StatsFake statsFake = playerCtrl.CharacterStatsFake;

            if (playerCtrl.CardCharacter.attackTypeCard == attackType)
            {
                statsFake.Attack = Mathf.RoundToInt(statsFake.Attack * (1 + increasePercentage));
            }
            // Tăng các chỉ số khác tương tự nếu cần

            playerCtrl.ApplyTemporaryStats(statsFake);

            Debug.Log("Warrior: " + statsFake.Attack);
        }
    }
}