using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardTowerSpawner : Spawner
{
    private static GuardTowerSpawner instance;
    public static GuardTowerSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (GuardTowerSpawner.instance != null) Debug.LogError("Onlly 1 GuardTowerSpawner Warning");
        GuardTowerSpawner.instance = this;
    }
}
