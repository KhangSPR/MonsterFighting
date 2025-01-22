using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltipGift : ItemTooltip
{
    ItemReward itemReward;
    ItemObject itemObject;
    private void Start()
    {
        if (itemObject != null)
        {
            Tooltip_Item.AddTooltip(transform, itemObject, null);
        }
        else
        {
            Tooltip_Item.AddTooltip(transform, null, itemReward);
        }
    }
    public void SetUIITemGift(ItemObject itemObject, ItemReward itemReward, int count)
    {
        if (itemReward != null)
        {
            this.itemReward = itemReward;
            this.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemReward.itemRarity);
            this.Avatar.sprite = itemReward.Image;


        }
        if (itemObject != null)
        {
            this.itemObject = itemObject;
            this.RawrRarity.material = RewardClaimManager.Instance.GetMaterial(itemObject.itemRarity);
            this.Avatar.sprite = itemObject.Sprite;
        }

        if (count != 0)
        {
            counttxt.text = "x" + count;
        }
        else
        {
            counttxt.text = "";
        }
    }
}
