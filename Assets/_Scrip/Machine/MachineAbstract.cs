using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineAbstract : SaiMonoBehaviour
{
    [Header("Guard Abtract")]
    [SerializeField] protected MachineCtrl machineCtrl;
    public MachineCtrl MachineCtrl { get => machineCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadMachineCtrl();
    }

    protected virtual void LoadMachineCtrl()
    {
        if (this.machineCtrl != null) return;
        this.machineCtrl = transform.parent.GetComponent<MachineCtrl>();
        Debug.Log(transform.name + ": LoadMachineCtrl", gameObject);
    }
}
