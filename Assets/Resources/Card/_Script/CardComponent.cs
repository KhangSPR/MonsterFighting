using UnityEngine;

public class CardComponent : ScriptableObject
{
    public string nameCard;
    public float cardRefresh;
    public int price;

    public Sprite frame;

    public Sprite background;
    public Sprite avatar;

    public CardComponent(string Name,float CardRefresh, int Price, Sprite Frame, Sprite Background, Sprite Avatar)
    {
        this.nameCard = Name;
        this.cardRefresh = CardRefresh;
        this.price = Price;
        this.frame = Frame;
        this.background = Background;
        this.avatar = Avatar;
    }

}
