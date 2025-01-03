using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemTooltip : MonoBehaviour
{
    [Header("UI Element Abstract")]
    [SerializeField] protected Image avatarImg;
    public Image AvatarImg => avatarImg;
    [SerializeField] protected TMP_Text counttxt;
    public TMP_Text CountTxt => counttxt;
}
