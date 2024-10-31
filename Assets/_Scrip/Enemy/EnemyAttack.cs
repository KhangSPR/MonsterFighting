using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : ObjAttack
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player" /*|| other.transform.parent.tag == "Castle"*/)
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
        if (listObjAttacks.Contains(other.transform.parent))
        {
            listObjAttacks.Remove(other.transform.parent);

            if (listObjAttacks.Count == 0)
            {
                checkCanAttack = false;

                this.ResetCollider(); //Reset Collider
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
