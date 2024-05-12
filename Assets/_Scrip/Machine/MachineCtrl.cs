using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class MachineCtrl : SaiMonoBehaviour
{
    [Header("Machine Ctrl")]
    [SerializeField] protected DamageSender damSender;
    public DamageSender DamageSender => damSender;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDamageSender();
    }
    protected virtual void LoadDamageSender()
    {
        if (this.damSender != null) return;
        this.damSender = transform.GetComponentInChildren<DamageSender>();
        Debug.Log(gameObject.name + ": LoadDamageSender" + gameObject);
    }
}
