using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class ObjMelee : AbstractCtrl
{
    [SerializeField] protected bool checkCanAttack = false;
    public bool CheckCanAttack { get { return checkCanAttack; } set { checkCanAttack = value; } }

    [SerializeField] protected List<Transform> listObjAttacks = new List<Transform>();
    public List<Transform> ListObjAttacks => listObjAttacks;

    [SerializeField]
    protected bool detectedFirstCollision = false;
    public bool DetectedFirstCollision => detectedFirstCollision;


    [Header("Abstract Model")]
    [SerializeField] protected BoxCollider2D boxCollider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBoxCollider2D();
    }
    protected virtual void LoadBoxCollider2D()
    {
        if (this.boxCollider != null) return;
        this.boxCollider = GetComponent<BoxCollider2D>();
        this.boxCollider.isTrigger = true;
        Debug.Log(transform.name + ": LoadBoxCollider2D", gameObject);
    }
    protected abstract void OnTriggerEnter2D(Collider2D other);
    protected abstract void OnTriggerExit2D(Collider2D other);

    protected void ResetCollider()
    {
        boxCollider.enabled = false;
        boxCollider.enabled = true;
    }
    public abstract Transform GetTransFromFirstAttack();
}
