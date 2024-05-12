using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardBtn : BaseBtn
{
    [SerializeField]
    protected GameObject cardPrefabSet;
    public GameObject CardPrefabSet { get { return cardPrefabSet; } set { cardPrefabSet = value; } }

    public override GameObject CardPrefabInstance
    {
        get { return cardPrefabSet; }
    }
    [SerializeField]
    protected GameObject selectButton;
    public GameObject SelectButton
    {
        get { return selectButton; }
    }
    //Card Data
    public CardCharacter CardCharacter;
    //Button
    [SerializeField] Button btn;
    [SerializeField] CardRefresh cardRefresh;
    public CardRefresh CardRefresh => cardRefresh;

    public override GameObject PlaceAbstract(Transform tileTransform)
    {
        // Hành vi khi đặt tháp (TowerBtn)
        GameObject towerObj = Instantiate(CardPrefabInstance, tileTransform.position, Quaternion.identity);

        towerObj.GetComponent<PlayerCtrl>().SetCardTower(CardCharacter);

        towerObj.SetActive(true);

        return towerObj;
    }
    protected override void Start()
    {
        base.Start();
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        if (cardRefresh.isCoolingDown) return;

        GameManager.Instance.PickButton(this, cardRefresh);
    }
}
