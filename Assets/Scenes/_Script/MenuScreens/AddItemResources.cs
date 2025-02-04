using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AddItemResources : MonoBehaviour
{
    [SerializeField] GameObject shopObj;
    [SerializeField] TabsManager tabsManager;

    public void ShowTabShop(int index)
    {
        shopObj.SetActive(true);
        if (index == 0) //Resources
        {
            tabsManager.SwitchToTab(1);
        }
        else //Ruby
        {
            tabsManager.SwitchToTab(2);
        }
    }
}
