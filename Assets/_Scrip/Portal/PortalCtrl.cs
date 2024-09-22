using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCtrl : AbilityPointAbstract
{
    [Header("PortalCtrl")]
    [SerializeField] protected ObjAppearSmall objAppearSmall;
    public ObjAppearSmall ObjAppearSmall => objAppearSmall;

    protected override void OnEnable()
    {
        base.OnEnable();

        this.EnableObject();
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        this.DisableObject();
    }
    protected void EnableObject()
    {
        if(this.abilities.AbilitySummon is AbilitySummonPortal abilitySummonPortal)
        {
            abilitySummonPortal.EnableObject();
        }
    }
    protected void DisableObject()
    {
        if (this.abilities.AbilitySummon is AbilitySummonPortal abilitySummonPortal)
        {
            abilitySummonPortal.DisableObject();
        }
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.loadObjAppearSmall();
    }

    protected virtual void loadObjAppearSmall()
    {
        if (this.objAppearSmall != null) return;
        this.objAppearSmall = transform.GetComponentInChildren<ObjAppearSmall>();
        Debug.Log(gameObject.name + ": loadObjAppearSmall" + gameObject);
    }
}
