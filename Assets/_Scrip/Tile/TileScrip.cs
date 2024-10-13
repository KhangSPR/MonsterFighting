using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class TileScrip : SaiMonoBehaviour
{
    //public Point GridPostion { get; private set; }
    [SerializeField]
    private bool isEmpty;
    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
    protected GameObject newObjSet;
    protected override void Start()
    {
        base.Start();
        isEmpty = true;
    }
    protected abstract void OnMouseOver();
    protected abstract void OnMouseExit();
    //private void PlaceTower()
    //{
    //    GameObject tower = (GameObject)Instantiate(GameManager.Instance.clickBtn.TowerPrefab, transform.position, Quaternion.identity);
    //    //tower.GetComponent<SpriteRenderer>().sortingOrder = GridPostion.Y;
    //    //Xếp layer chồng lên nhau
    //    tower.transform.SetParent(transform);

    //    isEmpty = false;

    //    GameManager.Instance.BuyTower();


    //}
    protected virtual void Place(Transform towerTransform)
    {
        if (GameManager.Instance.ClickBtn != null)
        {
            GameObject obj = GameManager.Instance.ClickBtn.PlaceAbstract(towerTransform);

            newObjSet = obj;

            if (obj != null)
            {
                //obj.transform.SetParent(transform);
                this.IsEmpty = false;
            }
        }
    }
}

