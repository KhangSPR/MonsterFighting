using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarSpawner : Spawner
{
    private static BarSpawner instance;
    public static BarSpawner Instance => instance;
    public static string HPBar = "CharacterBar";
    #region Classs Name Bar Spawner

    #endregion
    protected override void Awake()
    {
        base.Awake();
        //if (HPBarSpawner.instance != null) Debug.LogError("Onlly 1 HPBarSpawner Warning");
        BarSpawner.instance = this;
    }
}
