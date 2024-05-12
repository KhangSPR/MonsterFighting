using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSpawner : Spawner
{
    private static CardSpawner instance;
    public static CardSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (CardSpawner.instance != null) Debug.LogError("Onlly 1 CardSpawner Warning");
        CardSpawner.instance = this;
    }
}
