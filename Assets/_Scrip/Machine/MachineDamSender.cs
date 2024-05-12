using System.Collections;
using UnityEngine;

public class MachineDamSender : DamageSender
{
    [SerializeField] protected MachineCtrl machineCtrl;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMachineCtrl();
    }
    protected virtual void LoadMachineCtrl()
    {
        if (this.machineCtrl != null) return;
        this.machineCtrl = transform.parent.GetComponent<MachineCtrl>();
        Debug.Log(gameObject.name + ": LoadMachineCtrl" + gameObject);
    }
}
