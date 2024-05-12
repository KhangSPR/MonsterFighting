﻿using UnityEngine;
namespace UIGameDataManager
{
    public enum AttackType
    {
        ALL,
        Melee,       // Close-range physical attacks
        Ranged,      // Attacks from a distance
        Magic,       // Magical attacks
                     //Stealth,     // Sneaky and stealthy attacks



        // Add more types as needed

        //Melee represents close-range physical attacks, typically involving the use of melee weapons like swords, axes, or fists.
        //Ranged signifies attacks from a distance, often using ranged weapons such as bows, crossbows, or thrown weapons.
        //Magic encompasses attacks involving magical powers, spells, or abilities.
        //Stealth can represent attacks that rely on stealth and surprise, involving tactics like backstabbing or ambushes.
    }

    public enum Rarity
    {
        ALL,
        Common,   // Phổ biến
        Rare,     // Hiếm
        Epic,     // Kỳ diệu
        Legendary // Huyền bí
    }
    public enum CharacterClass
    {
        ALL,
        Paladin,    // Strong in melee combat, might use both magic and weapons
        Warrior,    // General melee-focused class
        Archer,     // Specialized in ranged attacks
        Mage,       // A generic magic user
        Witch,      // Specialized in dark or nature magic
                    //Barbarian,  // Focused on brute strength and melee combat
                    //Necromancer // Specialized in dark magic, particularly necromancy  


        // Add more classes as needed


        //Paladin and Warrior could be melee-focused classes with potential differences in their abilities or themes.
        //Archer is a ranged combat class, emphasizing skills with bows or other ranged weapons
        //Mage represents a more general magic user who might use a variety of magical abilities.
        //Witch is a specialized magic user, potentially focusing on dark or nature-based spells.
        //Barbarian is a class emphasizing brute strength and melee combat, similar to the Barbarian in the CharacterClass enum.
        //Necromancer is specialized in dark magic, particularly in the realm of necromancy.
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
        public SkillSO skill3;



        public Rarity cardRare;
        public AttackType cardAttack;
        public CharacterClass cardClass;
        //public GameObject characterVisualsPrefab;
        public CardStat cardStat;

        //public void UpdateData(int newLevel, string newCardName)
        //{
        //    level = newLevel;
        //    CardName = newCardName;
        //    // Update other properties as needed
        //}
        public AttackType GetAttackType()
        {
            return cardAttack;
        }

        public Rarity GetRarity()
        {
            return cardRare;
        }
        public CharacterClass GetCharacterClass()
        {
            return cardClass;
        }
        public CardCharacter(string Name,float CardRefresh, int Price, Sprite Frame, Sprite Background, Sprite Avatar,int level, int star, int BasePointsAttack, int BasePointsLife,
                         float BasePointsAttackSpeed, float BasePointsSpecialAttack,
                         string BioTitle, string Bio, SkillSO Skill1, SkillSO Skill2,
                         SkillSO Skill3, Rarity CardRare, AttackType CardAttack,
                         CharacterClass CardClass,/* GameObject CharacterVisualsPrefab,*/
                         CardStat CardStat) : base(Name, CardRefresh, Price, Frame, Background, Avatar)
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
            skill3 = Skill3;
            cardRare = CardRare;
            cardAttack = CardAttack;
            cardClass = CardClass;
            //characterVisualsPrefab = CharacterVisualsPrefab;
            cardStat = CardStat;
        }
    }
}