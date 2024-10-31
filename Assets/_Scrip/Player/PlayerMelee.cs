using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : ObjMelee
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Enemy")
        {
            if (!other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;

            if (!detectedFirstCollision)
            {
                checkCanAttack = true;
                detectedFirstCollision = true;
            }
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
                detectedFirstCollision = false;

                this.ResetCollider();//Reset Collider
            }
        }
    }
    public override Transform GetTransFromFirstAttack()
    {
        if (listObjAttacks.Count > 0)
            return listObjAttacks[0];
        return null;
    }
}
