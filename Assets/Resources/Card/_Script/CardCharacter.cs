//#nullable enable

using UnityEngine;

namespace UIGameDataManager
{
    public enum AttackCategory
    {
        ALL,
        Warrior,       // Close-range physical attacks
        Archer,      // Attacks from a distance
        Wizard,       // Magical attacks
        Tank
        //Stealth,     // Sneaky and stealthy attacks

        // Add more types as needed

        //Melee represents close-range physical attacks, typically involving the use of melee weapons like swords, axes, or fists.
        //Ranged signifies attacks from a distance, often using ranged weapons such as bows, crossbows, or thrown weapons.
        //Magic encompasses attacks involving magical powers, spells, or abilities.
        //Stealth can represent attacks that rely on stealth and surprise, involving tactics like backstabbing or ambushes.
    }

    public enum RarityCard
    {
        D,   // Level Card
        C,
        B,
        A,
        S,
        SS,
    }

    [CreateAssetMenu(fileName = "New Card", menuName = "Card/Create New Card Tower")]
    public class CardCharacter : CardComponent
    {
        [SerializeField] private Stats _characterStats;

        // Properties
        public Stats CharacterStats => _characterStats;

        public string bioTitle;
        [TextArea] public string bio;

        public SkillSO skill1;
        public SkillSO skill2;

        public GuildType guildType;
        public RarityCard rarityCard;
        public AttackCategory attackTypeCard;
        //public AttackTypeAnimation attackType;
        public GameObject characterVisualsPrefab;

        public AttackCategory GetAttackType()
        {
            return attackTypeCard;
        }

        public RarityCard GetRarity()
        {
            return rarityCard;
        }
    }
}
