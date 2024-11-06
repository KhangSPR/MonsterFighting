using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;

public class ArcherStatIncreaseStrategy : _IStatIncreaseStrategy
{
    private float increasePercentage = 0.25f; // Tăng 25% tốc đánh

    public void IncreaseStats(GameObject parent, AttackCategory attackType)
    {
        if (parent != null)
        {
            PlayerCtrl playerCtrl = parent.GetComponent<PlayerCtrl>();
            StatsFake statsFake = playerCtrl.CharacterStatsFake;

            if (playerCtrl.CardCharacter.attackTypeCard == attackType)
            {
                statsFake.AttackSpeed = Mathf.RoundToInt(statsFake.AttackSpeed * (1 + increasePercentage));
            }
            // Tăng các chỉ số khác tương tự nếu cần

            playerCtrl.ApplyTemporaryStats(statsFake);

            Debug.Log("Warrior: " + statsFake.AttackSpeed);
        }
    }
}
