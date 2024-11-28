using UnityEngine;

public abstract class TileScrip : SaiMonoBehaviour
{
    [SerializeField]
    private bool isEmpty;
    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
    [SerializeField]
    protected GameObject newObjSet;

    protected override void Start()
    {
        base.Start();
        isEmpty = true;
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
                if(playerCtrl != null)
                {
                    playerCtrl.ObjLand.SetLand(landIndex);
                    playerCtrl.AbstractModel.EffectCharacter.SetOrderLayerRenderer(landIndex);
                }

                this.IsEmpty = false;
            }
        }
    }
}
