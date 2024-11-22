using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : ObjMelee
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Player"/* || other.transform.parent.tag == "Castle"*/)
        {
            if (!other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;

            if (!detectedFirstCollision && listObjAttacks.Count == 0)
            {
                checkCanAttack = true;
                detectedFirstCollision = true;
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

                this.ResetCollider(); //Reset Collider
            }
        }
    }

    public override Transform GetTransFromFirstAttack()
    {
        Transform transform = listObjAttacks[0].GetComponent<PlayerCtrl>().TargetPosition;

        if (transform != null)
            return transform;
        return null;
    }
}
