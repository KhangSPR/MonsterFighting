using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCtrl : AbilityPointAbstract
{
    [Header("PortalCtrl")]
    [SerializeField] protected Abilities abilities;
    public Abilities Abilities => abilities;
    [SerializeField] protected ObjAppearSmall objAppearSmall;
    public ObjAppearSmall ObjAppearSmall => objAppearSmall;
    protected override void OnEnable()
    {
        base.OnEnable();
        this.abilities.AbilitySummonEnemy.EnableObject();
    }
    protected override void OnDisable()
    {
        this.abilities.AbilitySummonEnemy.DisableObject();

    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadAbilities();
        this.loadObjAppearSmall();
    }
    protected virtual void loadAbilities()
    {
        if (this.abilities != null) return;
        this.abilities = transform.GetComponentInChildren<Abilities>();
        Debug.Log(gameObject.name + ": loadAbilities" + gameObject);
    }
    protected virtual void loadObjAppearSmall()
    {
        if (this.objAppearSmall != null) return;
        this.objAppearSmall = transform.GetComponentInChildren<ObjAppearSmall>();
        Debug.Log(gameObject.name + ": loadObjAppearSmall" + gameObject);
    }
}
