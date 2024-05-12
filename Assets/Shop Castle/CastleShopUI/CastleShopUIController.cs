using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class CastleShopUIController : MonoBehaviour
{
    [SerializeField] private Transform castle_button_ui_prefabs;
    public Castle_Shop shop;
    public CastleSO choosing_castle;
    [SerializeField] Castle_Image castle_Image;
    //[SerializeField] GameIconsSO m_GameIconsData;


    //const string k_ResourcePath = "GameData/GameIcons";
    private void Awake()
    {
        //m_GameIconsData = Resources.Load<GameIconsSO>(k_ResourcePath);

        LoadShop();
    }
    public void UpdateShopInformation()
    {

    }
    public void LoadShop()
    {
        LoadButtonsShop();
        //LoadChoosenCastle();
        LoadCastlesInformations(0);

    }


    private void LoadChoosenCastle(CastleSO castle)
    {
        shop.choosen_item = castle;
    }
    public void SetChoosingCastle(int index)
    {
        choosing_castle = shop.csi[index].castle;

        LoadCastlesInformations(index);
        Debug.Log("index in shop = " + index);
    }
    public void SetChoosingCastle(CastleSO castle)
    {
        choosing_castle = castle;
        int indexInShop = -1;
        foreach (Castle_Shop_Item csi in shop.csi)
        {
            if (csi.castle == choosing_castle) indexInShop = shop.csi.IndexOf(csi);

        }

        LoadCastlesInformations(indexInShop);
        Debug.Log("index in shop = " + indexInShop);
    }

    public void SetChoosenCastle()
    {
        shop.choosen_item = choosing_castle;
        //transform.Find("Castle_Image").Find("Use_Castle_Button").GetComponent<Button>().interactable = false;
        castle_Image.Btn_Use_Castle_Button.interactable = false;
    }
    public void LoadCastlesInformations(int index)
    {
        Debug.Log("LoadCastlesInformations");

        //Castle_Image castle_image = transform.Find("Castle_Image").GetComponent<Castle_Image>();

        //UI
        castle_Image.SetCost(shop.csi[index].cost);

        castle_Image.SetUI(shop.csi[index].castle);

        castle_Image.CheckIs_Owned(shop.csi[index].castle.is_owned, shop.choosen_item == shop.csi[index].castle);

        castle_Image.LoadBlur();
    }

    private void LoadButtonsShop()
    {
        Transform initParent = transform.Find("Castle_Buttons/Viewport/ContentCastle_Buttons");
        foreach (Transform child in initParent)
        {
            Destroy(child.gameObject);
        }
        //if(initParent.childCount == 0)
        //{
        foreach (Castle_Shop_Item si in shop.csi)
        {
            Transform button = Instantiate(castle_button_ui_prefabs, initParent);
            button.GetComponent<ButtonCastle>().SetUIButtonCastle(si.castle);

            button.GetComponent<ChoosingCastleUI>().castleShopUIController = this;
        }
        //}

    }
}
