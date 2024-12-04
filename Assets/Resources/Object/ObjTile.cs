using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjTile : PlayerAbstract
{
    [SerializeField] protected TileTower tileTower;
    public TileTower TileTower => tileTower;

    public void SetTileTower(TileTower tileTower)
    {
        this.tileTower = tileTower;
    }
}
