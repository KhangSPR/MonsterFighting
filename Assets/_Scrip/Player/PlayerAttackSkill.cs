using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSkill : AbstractCtrl
{
    [SerializeField] protected bool checkCanAttack = false;
    public bool CheckCanAttack { get { return checkCanAttack; } set { checkCanAttack = value; } }
    [SerializeField] protected List<Transform> listObjAttacksSkill = new List<Transform>();
    public List<Transform> ListObjAttacksSkill => listObjAttacksSkill;
    [Header("Abstract Model")]
    [SerializeField] protected BoxCollider2D boxCollider;
    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Enemy")
        {
            ObjectCtrl objectCtrl = other.transform.parent.GetComponent<ObjectCtrl>();
            if (!objectCtrl.ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;
            if (objectCtrl.ObjectDamageReceiver.IsDead)
            {
                return;
            }
            checkCanAttack = true;
            if (!listObjAttacksSkill.Contains(other.transform.parent))
            {
                listObjAttacksSkill.Add(other.transform.parent);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent != null && listObjAttacksSkill.Contains(other.transform.parent))
        {
            listObjAttacksSkill.Remove(other.transform.parent);
            if (listObjAttacksSkill.Count == 0)
            {
                checkCanAttack = false;

                this.ResetCollider(); // Reset Collider
            }
        }
    }
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
    protected void ResetCollider()
    {
        boxCollider.enabled = false;
        boxCollider.enabled = true;
    }
}
