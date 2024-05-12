using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseButtonUI : MonoBehaviour
{
    [SerializeField] Transform purchaseSuccess_prefabs;
    [SerializeField] Transform purchaseFail_prefabs;
    [SerializeField] Image imageFade;
    public void OpenSuccessUI()
    {
        purchaseSuccess_prefabs.gameObject.SetActive(true);
        imageFade.gameObject.SetActive(true);
    }
    public void CloseSuccessUI()
    {
        purchaseSuccess_prefabs.gameObject.SetActive(false);
        imageFade.gameObject.SetActive(false);
    }
    public void OpenFailUI()
    {
        purchaseFail_prefabs.gameObject.SetActive(true);
        imageFade.gameObject.SetActive(true);

    }
    public void CloseFailUI()
    {
        purchaseFail_prefabs.gameObject.SetActive(false);
        imageFade.gameObject.SetActive(false);
    }
    public void Purchase()
    {
        uint money_data = GameObject.Find("GameDataManager").GetComponent<GameDataManager>().GameData.gold;
        Debug.Log(money_data);
        
        {
            CastleShopUIController shopUI = transform.parent.parent.GetComponent<CastleShopUIController>();
            CastleSO choosingCastle = transform.parent.parent.GetComponent<CastleShopUIController>().choosing_castle;

            int index = -1;
            foreach (Castle_Shop_Item csi in shopUI.shop.csi)
            {
                uint purchase_money = (uint)csi.cost;
                if (choosingCastle == csi.castle )
                {
                    if(purchase_money <= money_data)
                    {
                        index = shopUI.shop.csi.IndexOf(csi);
                        choosingCastle.is_owned = true;
                        GameObject.Find("GameDataManager").GetComponent<GameDataManager>().GameData.gold -= purchase_money;
                        transform.parent.parent.GetComponent<CastleShopUIController>().LoadCastlesInformations(index);
                        OpenSuccessUI();
                        Debug.Log("Purchase Success");
                    }
                    else
                    {
                        Debug.Log("Purchase Failed");
                        OpenFailUI();
                    }
                } 
            }

            
        }
    }

}
