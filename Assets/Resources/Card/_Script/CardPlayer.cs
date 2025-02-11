using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public enum GendersType
{
    Male,
    FeMale
}
[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create New Card Player")]
public class CardPlayer : CardCharacter
{ 
    [Space]
    [Header("Card Player")]
    public GendersType Genders;
    public Sprite AvatarPlayer;
    public Sprite ModlePlayer;
    //
    
}
