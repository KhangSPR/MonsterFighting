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

        public RarityCard rarityCard;
        public AttackCategory attackTypeCard;
        //public AttackTypeAnimation attackType;
        public GameObject characterVisualsPrefab;

        public CardCharacter(string Name, float CardRefresh, int Price, Sprite Frame, Sprite Background, Sprite Avatar, Stats stats,
                             string BioTitle, string Bio, SkillSO Skill1, SkillSO Skill2,
                             RarityCard CardRare, AttackCategory CardAttack,
                             GameObject CharacterVisualsPrefab) : base(Name, CardRefresh, Price, Frame, Background, Avatar)
        {
            _characterStats = stats;
            bioTitle = BioTitle;
            bio = Bio;
            rarityCard = CardRare;
            attackTypeCard = CardAttack;
            characterVisualsPrefab = CharacterVisualsPrefab;

            // Set skills based on rarity
            SetSkillsBasedOnRarity(Skill1, Skill2);
        }

        private void SetSkillsBasedOnRarity(SkillSO skill1, SkillSO skill2)
        {
            this.skill1 = skill1;
            this.skill2 = (rarityCard == RarityCard.D || rarityCard == RarityCard.C) ? null : skill2;
        }

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
