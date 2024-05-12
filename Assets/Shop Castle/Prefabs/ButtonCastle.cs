using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCastle : MonoBehaviour
{
    [SerializeField] Image icon;
    public Image Icon => icon;

    [SerializeField] Image frameFlag;
    public Image FrameFlag => frameFlag;

    [SerializeField] TMP_Text nameCastle;
    public TMP_Text NameCastle => nameCastle;

    public void SetUIButtonCastle(CastleSO castle)
    {
        this.icon.sprite = castle.icon;

        icon.color = castle.GetColorCastle(castle.colorCastle);

        this.frameFlag.sprite = castle.frameFlag;
        this.nameCastle.text = castle.castle_name;
    }    


}
