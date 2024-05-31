using UnityEngine;

public enum CardClassMachine
{
    ALL,
    Machine,
    TNT,
    Cannon
    // Add more types as needed
}

[CreateAssetMenu(fileName = "New Card", menuName = "Card/Create New Card Machine")]
public class CardMachine : CardComponent
{
    public CardMachine(string Name,float CardRefresh, int Price, Sprite Frame, Sprite Background, Sprite Avatar) : base(Name,CardRefresh, Price, Frame, Background, Avatar)
    {
    }
}
