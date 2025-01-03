using UnityEngine;

public class ItemTooltipReward : ItemTooltip
{
    [Space(2)]
    [Header("UI Reward")]
    [SerializeField] GameObject tickObj;
    public GameObject Tick => tickObj;

    [Space]
    [SerializeField]
    ItemReward itemReward;
    public ItemReward ItemReward { get { return itemReward; } set { itemReward = value; } }
    private void Start()
    {
        if(itemReward != null)
        {
            Tooltip_Item.AddTooltip(transform, null, itemReward);
        }
    }
}
