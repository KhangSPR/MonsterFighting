using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawner : Spawner
{
    private static PortalSpawner instance;
    public static PortalSpawner Instance => instance;

    public static string PortalOne = "Portal_1"; // bullet 1
    public static string PortalTwo = "Portal_2"; // bullet 1
    public static string PortalThree = "Portal_3"; // bullet 1
    protected override void Awake()
    {
        base.Awake();
        //if (PortalSpawner.instance != null) Debug.LogError("Onlly 1 PortalSpawner Warning");
        PortalSpawner.instance = this;
    }
}
