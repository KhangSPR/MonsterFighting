using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : ObjAttack
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player")
        {

            if (!other.transform.parent.GetComponent<ObjectCtrl>().ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;


            checkCanAttack = true;

            if (!listObjAttacks.Contains(other.transform.parent))
            {
                listObjAttacks.Add(other.transform.parent);
            }
        }
        else if (other.transform.parent.tag == "Castle")
        {
            checkCanAttack = true;

            CastleCtrl castleCtrl = other.transform.parent.GetComponent<CastleCtrl>();

            //Set Target City
            this.enemyCtrl.ObjMoveIntheCity.SetTargetMoveCity(castleCtrl.ObjMove);

            //Move y
            //CheckDeadCastle(castleCtrl);

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
    protected void CheckDeadCastle(CastleCtrl castleCtrl)
    {
        if (castleCtrl.CastleDamageReceiver.IsDead)
        {
            //Obj Move In The City 
            this.OnDeadCastle();
        }
    }
    public void OnDeadCastle()
    {
        //Obj Move In The City 
        this.enemyCtrl.ObjMovement.MoveSpeed = 0f;
        this.listObjAttacks.Clear();
        this.checkCanAttack = false;
        this.enemyCtrl.ObjMoveIntheCity.IsMoveInTheCity = true;

        Debug.Log("OnDeadCastle");
    }
}
