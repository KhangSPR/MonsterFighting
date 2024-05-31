using UnityEngine;
namespace UIGameDataManager
{
    public enum AttackType
    {
        ALL,
        Melee,       // Close-range physical attacks
        Ranged,      // Attacks from a distance
        Witch,       // Magical attacks
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
        // Additional stats
        public int Level;
        public int Star;

        public int basePointsAttack;
        public int basePointsLife;
        public float basePointsAttackSpeed;
        public float basePointsSpecialAttack;


        public string bioTitle;
        [TextArea] public string bio;

        // skill1 unlocked at level 0, skill2 unlocked at level 3, skill3 unlocked at level 6
        public SkillSO skill1;
        public SkillSO skill2;



        public RarityCard rarityCard;
        public AttackType attackTypeCard;
        public GameObject characterVisualsPrefab;
        public RaritySO cardStat;

        //public void UpdateData(int newLevel, string newCardName)
        //{
        //    level = newLevel;
        //    CardName = newCardName;
        //    // Update other properties as needed
        //}
        public AttackType GetAttackType()
        {
            return attackTypeCard;
        }

        public RarityCard GetRarity()
        {
            return rarityCard;
        }
        public CardCharacter(string Name,float CardRefresh, int Price, Sprite Frame, Sprite Background, Sprite Avatar,int level, int star, int BasePointsAttack, int BasePointsLife,
                         float BasePointsAttackSpeed, float BasePointsSpecialAttack,
                         string BioTitle, string Bio, SkillSO Skill1, SkillSO Skill2,
                         RarityCard CardRare, AttackType CardAttack,
                          GameObject CharacterVisualsPrefab,
                         RaritySO CardStat) : base(Name, CardRefresh, Price, Frame, Background, Avatar)
        {
            Level = level;
            Star = star;
            basePointsAttack = BasePointsAttack;
            basePointsLife = BasePointsLife;
            basePointsAttackSpeed = BasePointsAttackSpeed;
            basePointsSpecialAttack = BasePointsSpecialAttack;
            bioTitle = BioTitle;
            bio = Bio;
            skill1 = Skill1;
            skill2 = Skill2;
            rarityCard = CardRare;
            attackTypeCard = CardAttack;
            characterVisualsPrefab = CharacterVisualsPrefab;
            cardStat = CardStat;
        }
    }
}