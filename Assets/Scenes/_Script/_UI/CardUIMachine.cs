using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardUIMachine : MonoBehaviour
{
    [SerializeField] int idCard;
    private void OnEnable()
    {
        Button button = gameObject.GetComponent<Button>();
        if (button == null)
        {
            button = gameObject.AddComponent<Button>();
        }

        // Thêm hàm OnButtonClick() vào sự kiện click của Button
        button.onClick.AddListener(OnButtonClick);
    }
    public void SetCardInfo(GameObject cardObject, CardMachine card)
    {
        //idCard = card.id;
        ////IMG Background Rare
        //cardObject.GetComponent<Image>().sprite = card.BackgroundRare;
        ////IMG CARD 
        //cardObject.transform.Find("ImgBackground").GetComponent<Image>().sprite = card.backgroundIMG;
        //cardObject.transform.Find("ImgBackground/IMGCard").GetComponent<Image>().sprite= card.icon;

        ////IMG CARD Name
        //cardObject.transform.Find("IMGName").GetComponent<Image>().sprite = card.backgroundText;
        //cardObject.transform.Find("IMGName/TextName").GetComponent<TMP_Text>().text = card.CardName;

        ////IMG Rare
        //cardObject.transform.Find("Rare").GetComponent<Image>().sprite = card.IMGRare;

        ////Text Description
        //cardObject.transform.Find("Info/TextInfo").GetComponent<TMP_Text>().text = card.title;

    }
    protected void OnButtonClick()
    {
        CardUIPanelManager.Instance.CheckCardTypeAndProcess(idCard,this);
    }
}
