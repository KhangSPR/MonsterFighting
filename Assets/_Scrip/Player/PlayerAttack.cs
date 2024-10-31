using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : ObjAttack
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Enemy")
        {
            if (!other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;

            checkCanAttack = true;

            if (!listObjAttacks.Contains(other.transform.parent))
            {
                listObjAttacks.Add(other.transform.parent);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent != null && listObjAttacks.Contains(other.transform.parent))
        {
            listObjAttacks.Remove(other.transform.parent);
            if (listObjAttacks.Count == 0)
            {
                checkCanAttack = false;

                this.ResetCollider();//Reset Collider
            }
        }
    }

    public override Transform GetTransFromFirstAttack()
    {
        Transform transform = listObjAttacks[0].GetComponent<ObjectCtrl>().TargetPosition;

        if (transform != null)
            return transform;
        return null;
    }
}
