using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineSpawner : Spawner
{
    private static MachineSpawner instance;
    public static MachineSpawner Instance => instance;
    protected override void Awake()
    {
        base.Awake();
        //if (MachineSpawner.instance != null) Debug.LogError("Onlly 1 MachineSpawner Warning");
        MachineSpawner.instance = this;
    }
}
