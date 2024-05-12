using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosingCastleUI : MonoBehaviour
{
    public CastleShopUIController castleShopUIController;

    public void SetChoosingCastle()
    {
        int index = transform.GetSiblingIndex();
        castleShopUIController.SetChoosingCastle(index);
        //Debug.Log("Choosing index " + index);
        //Debug.Log("Choosing castle = " + transform.parent.parent.GetComponent<CastleShopUIController>().choosing_castle.castle_name);
    }
}
