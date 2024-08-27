using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorCastle
{
    Black, Red, Blue, Green, Yellow
}

[System.Serializable,CreateAssetMenu]
public class CastleSO : ScriptableObject
{
    //[Header("private variable")]
    //[SerializeField,ReadOnlyInspector] private int _max_hp = 2;
    [Header("public variable")]
    public string castle_name;
    [TextArea] public string castle_description;
    [TextArea] public string castle_history;
    public Sprite backGround;
    public Sprite frameGround;
    public Sprite avatar;
    public ColorCastle colorCastle;
    public Sprite icon;
    public Sprite frameFlag;
    public bool is_owned = false;
    public Ability ability;
    public ShopItemType contentType;
    public Color GetColorCastle(ColorCastle colorCastle)
    {
        switch (colorCastle)
        {
            case ColorCastle.Black:
                return Color.black;
            case ColorCastle.Red:
                return Color.red;
            case ColorCastle.Blue:
                return Color.blue;
            case ColorCastle.Green:
                return Color.green;
            case ColorCastle.Yellow:
                return Color.yellow;


            default:
                return Color.white; // hoặc màu khác nếu cần
        }
    }
    // SC (gold) costs HC (gems); HC (gems) costs real USD; HealthPotion costs SC (gold); LevelUpPotion costs HC (gems)
    public CurrencyType CostInCurrencyType
    {
        get
        {
            switch (contentType)
            {
                case (ShopItemType.CastleGold):
                    return CurrencyType.Gold;

                //case (ShopItemType.CastleRuby):
                //    return CurrencyType.Ruby;
                default:
                    return CurrencyType.Gold;
            }
        }
    }

}
