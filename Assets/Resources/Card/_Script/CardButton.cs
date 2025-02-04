using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] Image frameAvatar;
    [SerializeField] Image frameName;
    [SerializeField] Image Background;
    [SerializeField] TMP_Text textName;

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
        if (GameManager.Instance.AreFlagsSet(GameStateFlags.ClickInventory | GameStateFlags.ClickHoverRemove | GameStateFlags.ClickHoverMove | GameStateFlags.StarCondition)) return;
        if (cardRefresh.isCoolingDown) return;

        GameManager.Instance.PickButton(this, cardRefresh);

        Debug.Log("Click");
    }
    public void SetUICard(CardCharacter cardCharacter)
    {
        //Settings
        frameAvatar.sprite = cardCharacter.frame;
        frameName.sprite = cardCharacter._frameCardName;
        Background.sprite = cardCharacter.background;
        textName.text = cardCharacter.nameCard;
        // Set Prefab Instance
        //newCardObject.GetComponent<CardBtn>().CardPrefabSet = PlayerSpawner.Instance.GetGameobjectPrefabByName(cardCharacterData.nameCard);
        CardCharacter = cardCharacter;
        Sprite = cardCharacter.avatar;
        Price = cardCharacter.price;
        CardRefresh.cooldownDuration = cardCharacter.cardRefresh;
    }
}
