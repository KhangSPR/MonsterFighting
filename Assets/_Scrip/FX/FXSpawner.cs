using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSpawner : Spawner
{
    private static FXSpawner instance;
    public static string SmokeOne = "Smoke_1"; // bullet 1
    public static string SmokeTwo = "Smoke_2"; // bullet 2
    public static string ImpactOne = "Impact_1"; //Impact 1
    public static string ImpactTwo = "Impact_2"; //Impact 2
    public static string BreackOne = "Breack_1"; //Breack 1
    public static string textDamage = "TextDamage"; //Impact 2
    public static string stun = "Stun";
    public static FXSpawner Instance { get => instance; }
    protected override void Awake()
    {
        base.Awake();
        //if (FXSpawner.instance != null) Debug.LogError("Onlly 1 FXSpawner Warning");
        FXSpawner.instance = this;
    }
}
