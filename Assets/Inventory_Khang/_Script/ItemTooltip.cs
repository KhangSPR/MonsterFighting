using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemTooltip : MonoBehaviour
{
    [Header("UI Element Abstract")]
    [SerializeField] protected RawImage rawRarity;
    public RawImage RawrRarity => rawRarity;
    [SerializeField] protected Image avatar;
    public Image Avatar => avatar;
    [SerializeField] protected TMP_Text counttxt;
    public TMP_Text CountTxt => counttxt;
}
