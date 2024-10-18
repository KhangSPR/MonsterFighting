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
    public static string MagicVortex = "Magic Vortex";
    public static string stun = "Stun";
    public static string BallOfDarkness = "Ball Of Darkness";
    public static string VenomousExplosionSphere = "Venomous Explosion Sphere";

    public static FXSpawner Instance { get => instance; }
    protected override void Awake()
    {
        base.Awake();
        //if (FXSpawner.instance != null) Debug.LogError("Onlly 1 FXSpawner Warning");
        FXSpawner.instance = this;
    }
    #region FX Text ... 
    public void SendFXText(int dame, SkillType skillType, Transform hitPos, Quaternion rotation)
    {
        //Vector3 hitPos = transform.position;
        //Quaternion hitRot = transform.rotation;

        //this.CreateImpactFX(hitPos, hitRot);
        this.CreateTextDamageFX(dame, hitPos.position, skillType);
    }
    protected virtual void CreateTextDamageFX(int dame, Vector3 hitPos, SkillType skillType)
    {
        string fxName = this.GetTextDamageFX();
        Transform fxObj = Spawn(fxName, hitPos, Quaternion.identity);
        TextDamage textDamage = fxObj.GetComponent<TextDamage>();
        textDamage.DoAnimation(dame, skillType);
        fxObj.gameObject.SetActive(true);
    }

    protected virtual string GetTextDamageFX()
    {
        return textDamage;
    }
    #endregion
}
