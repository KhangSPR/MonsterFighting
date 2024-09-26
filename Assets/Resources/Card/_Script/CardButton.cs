using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CardButton : BaseBtn
{
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
    [SerializeField] ImageRefresh cardRefresh;
    public ImageRefresh CardRefresh => cardRefresh;

    public override GameObject PlaceAbstract(Transform tileTransform)
    {
        // Hành vi khi đặt tháp (TowerBtn)
        Transform towerObj = PlayerSpawner.Instance.Spawn(CardCharacter.name, tileTransform.position, Quaternion.identity);

        towerObj.GetComponent<PlayerCtrl>().SetCardTower(CardCharacter);

        towerObj.gameObject.SetActive(true);

        return towerObj.gameObject;
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

        Debug.Log("Click");
    }
}
