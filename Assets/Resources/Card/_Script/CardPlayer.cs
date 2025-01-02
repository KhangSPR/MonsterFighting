using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public enum GendersType
{
    Male,
    FeMale
}
[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create New Card Player")]
public class CardPlayer : CardComponent
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
    public AttackCategory GetAttackType()
    {
        return attackTypeCard;
    }

    public RarityCard GetRarity()
    {
        return rarityCard;
    }
    [Space]
    [Header("Card Player")]
    public GendersType Genders;
    public Sprite AvatarPlayer;
    //
    
}
