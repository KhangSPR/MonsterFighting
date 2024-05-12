using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MachineImpact : MachineAbstract
{
    [Header("Machine Impact")]
    public bool isCollider = false;

    [SerializeField] protected Rigidbody2D _rigidbody;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigibody();
    }

    protected virtual void LoadRigibody()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.isKinematic = true;
        Debug.Log(transform.name + ": LoadRigibody", gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            isCollider = true;
            //this.machineCtrl.DamageSender.SendDamageOverTime(other.transform, isCollider);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent.CompareTag("Enemy"))
        {
            isCollider = false;
        }
    }
}
