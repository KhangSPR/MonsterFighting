using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseCastleButtonUI : MonoBehaviour
{
    [SerializeField] CastleShopUIController castleShopUIController;
    public void SetUseCastle()
    {
        castleShopUIController.SetChoosenCastle();
    }


}
