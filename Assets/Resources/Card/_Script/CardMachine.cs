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

}
