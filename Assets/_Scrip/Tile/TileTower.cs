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
    public void IsSwap(GameObject swapObj)
    {
        if (!IsEmpty)
        {
            Hover.Instance.Deactivate();
            GameManager.Instance.SetFlag(GameStateFlags.ClickTile, false);
            return;

        }
        else
        {
            PlayerCtrl playerCtrl = swapObj.GetComponent<PlayerCtrl>();
            playerCtrl.ObjTile.TileTower.IsEmpty = true;
            playerCtrl.ObjTile.TileTower.newObjSet = null;
            playerCtrl.ObjLand.SetLand(landIndex);
            playerCtrl.ObjTile.SetTileTower(this);
            newObjSet = swapObj;

            newObjSet.transform.position = transform.position;

            Hover.Instance.Deactivate();

            GameManager.Instance.CurrentMove -= 1;
            GameManager.Instance.SetFlag(GameStateFlags.ClickTile, false);

            Debug.Log("Swappp");

            IsEmpty = false;
        }
    }
    protected virtual void Place(Transform towerTransform, int landIndex)
    {
        if (GameManager.Instance.ClickBtn != null)
        {
            GameObject obj = GameManager.Instance.ClickBtn.PlaceAbstract(towerTransform);
            newObjSet = obj;

            if (obj != null)
            {
                PlayerCtrl playerCtrl = newObjSet.GetComponent<PlayerCtrl>();
                if (playerCtrl != null)
                {
                    playerCtrl.ObjLand.SetLand(landIndex);
                    playerCtrl.AbstractModel.EffectCharacter.SetOrderLayerRenderer(landIndex);
                    playerCtrl.ObjTile.SetTileTower(this);
                }

                this.IsEmpty = false;
            }
        }
    }
}
