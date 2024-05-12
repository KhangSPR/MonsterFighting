using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopComponent : MonoBehaviour
{
    [SerializeField] ShopController shopController;
    private void OnEnable()
    {
        //shopController.UpdateViewItemCards();
        //shopController.UpdateViewItems();
    }
    private void OnDisable()
    {
        
    }
}
