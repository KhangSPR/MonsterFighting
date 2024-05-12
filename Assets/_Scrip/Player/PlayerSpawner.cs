using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : Spawner
{
    private static PlayerSpawner instance;
    public static PlayerSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (PlayerSpawner.instance != null) Debug.LogError("Onlly 1 PlayerSpawner Warning");
        PlayerSpawner.instance = this;
    }

}