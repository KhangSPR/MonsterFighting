using UIGameDataManager;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class WarriorStatIncreaseStrategy : _IStatIncreaseStrategy
{
    private float increasePercentage = 0.25f; // Tăng 25% sức tấn công

    public void IncreaseStats(GameObject parent, AttackType attackType)
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
