using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawn : SaiMonoBehaviour
{
    [SerializeField]
    private bool isEmpty;

    public bool IsEmpty
    {
        get { return isEmpty; }
        set { isEmpty = value; }
    }
}
