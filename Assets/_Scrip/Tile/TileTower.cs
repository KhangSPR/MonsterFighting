 using UnityEngine;
using UnityEngine.EventSystems;

public class TileTower : TileScrip
{
    [SerializeField] private int landIndex;
    public int LandIndex => landIndex;
    public void SetLandIndex(int landIndex)
    {
        this.landIndex = landIndex;
    }
    public void IsActive()
    {
        if(!IsEmpty)
        {
            if (newObjSet == null) return;

            if (!newObjSet.activeSelf)
            {
                ActionPlace();
            }
        }
        else
        {
            ActionPlace();
        }
    }
    private void ActionPlace()
    {
        Place(transform, landIndex); // Truyền landIndex vào Place
        GameManager.Instance.BuyCard();
        GameManager.Instance.CardRefresh.StartCooldown();
    }
}
