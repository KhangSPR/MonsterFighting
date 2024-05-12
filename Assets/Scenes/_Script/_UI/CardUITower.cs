using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class CardUITower : MonoBehaviour
{
    [Header("Frame")]
    [SerializeField] Image frame;

    [Header("Transform")]
    [SerializeField] Transform Top;
    [SerializeField] Transform Center;
    [SerializeField] Transform Bottom;

    [Header("Star")]
    [SerializeField] GameObject StarPrefabs;
    [SerializeField] Transform StarHolder;

    public int idCard;
    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        // Thêm hàm OnButtonClick() vào sự kiện click của Button
        button.onClick.AddListener(OnButtonClick);
    }
    public void SetCardInfo(GameObject cardObject, CardCharacter card)
    {
        //Frame
        frame.sprite = card.frame;
        //Top
        Top.Find("BackGround").GetComponent<Image>().sprite = card.background;
        //Top.Find("Avatar").GetComponent<Image>().sprite = card.avatar;
        //Top.Find("Rare").GetComponent<Image>().sprite = card.rareTop;

        //Center
        //Center.Find("BackGround").GetComponent<Image>().sprite = card.rareCenter;
        Center.Find("Text").GetComponent<TMP_Text>().text = card.nameCard;

        //Bottom
        Bottom.Find("TextInfo").GetComponent<TMP_Text>().text = card.bioTitle;

        // Check Star
        for (int i = 0; i < card.Star; i++)
        {
            Instantiate(StarPrefabs, StarHolder);
        }
    }
    protected void OnButtonClick()
    {
        CardUIPanelManager.Instance.CheckCardTypeAndProcess(idCard, this);
    }
}
