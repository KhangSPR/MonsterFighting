using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineBtn : BaseBtn
{
    [SerializeField]
    private int towerPrice; 
    [SerializeField]
    protected GameObject towerPrefab;
    public GameObject TowerPrefab { get => towerPrefab; }
    [SerializeField]
    protected Text priceTxt;
    public override GameObject CardPrefabInstance
    {
        get { return TowerPrefab; }
    }
    //protected override void Start()
    //{
    //    base.Start();
    //    price = towerPrice;
    //    if (priceTxt != null)
    //    {
    //        priceTxt.text = Price + "$";
    //    }
    //}
    public override GameObject PlaceAbstract(Transform tileTransform)
    {
        // Hành vi khi đặt tháp (TowerBtn)
        GameObject towerObj = Instantiate(CardPrefabInstance, tileTransform.position, Quaternion.identity);

        return towerObj;
    }
}
