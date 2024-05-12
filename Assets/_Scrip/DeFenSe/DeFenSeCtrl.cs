using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeFenSeCtrl : SaiMonoBehaviour
{
    [Header("DeFenSe Ctrl")]
    [SerializeField] protected DefenseLookAtDistance defenseLookAtDistance;
    public DefenseLookAtDistance DefenseLookAtDistance => defenseLookAtDistance;
    [SerializeField] protected DefenseShooter defenseShooter;
    public DefenseShooter DefenseShooter => defenseShooter;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadLookAtDistance();
        this.LoadShootByDistance();
    }
    protected virtual void LoadLookAtDistance()
    {
        if (this.defenseLookAtDistance != null) return;
        this.defenseLookAtDistance = transform.GetComponentInChildren<DefenseLookAtDistance>();
        Debug.Log(gameObject.name + ": LoadLookAtDistance" + gameObject);
    }
    protected virtual void LoadShootByDistance()
    {
        if (this.defenseShooter != null) return;
        this.defenseShooter = transform.GetComponentInChildren<DefenseShooter>();
        Debug.Log(gameObject.name + ": LoadShootByDistance" + gameObject);
    }
}
