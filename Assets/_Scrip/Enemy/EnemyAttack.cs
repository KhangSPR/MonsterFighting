using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack : ObjAttack
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent == null) return; // Kiểm tra parent có tồn tại không


        if (other.transform.parent.tag == "Skill")
        {
            checkCanAttack = true;

            // Kiểm tra xem SkillCtrl đã tồn tại trong danh sách chưa
            SkillCtrl skillCtrl = other.transform.parent.GetComponent<SkillCtrl>();
            if (!this.enemyCtrl.TargetSkillScript.listSkillCtrl.Contains(skillCtrl))
            {
                this.enemyCtrl.TargetSkillScript.listSkillCtrl.Add(skillCtrl);
                Debug.Log("Add Skill");
            }

            Debug.Log("Trigger Skill");
            return;
        }

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

            // Set Target City
            this.enemyCtrl.ObjMoveIntheCity.SetTargetMoveCity(castleCtrl.ObjMove);

            if (!listObjAttacks.Contains(other.transform.parent))
            {
                listObjAttacks.Add(other.transform.parent);
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra nếu parent tồn tại
        if (other.transform.parent == null) return;

        // Xử lý xóa khỏi listObjAttacks
        if (listObjAttacks.Contains(other.transform.parent))
        {
            listObjAttacks.Remove(other.transform.parent);

            if (listObjAttacks.Count == 0)
            {
                checkCanAttack = false;
                this.ResetCollider(); // Reset Collider
            }
        }

        // Xử lý xóa khỏi TargetSkillScript.listSkillCtrl
        if (other.transform.parent.CompareTag("Skill"))
        {
            SkillCtrl skillCtrl = other.transform.parent.GetComponent<SkillCtrl>();
            if (skillCtrl != null && this.enemyCtrl != null &&
                this.enemyCtrl.TargetSkillScript != null &&
                this.enemyCtrl.TargetSkillScript.listSkillCtrl != null)
            {
                if (this.enemyCtrl.TargetSkillScript.listSkillCtrl.Contains(skillCtrl))
                {
                    this.enemyCtrl.TargetSkillScript.listSkillCtrl.Remove(skillCtrl);

                    if (this.enemyCtrl.TargetSkillScript.listSkillCtrl.Count == 0)
                    {
                        checkCanAttack = false;
                        this.ResetCollider(); // Reset Collider
                    }
                    Debug.Log("Remove Skill");
                }
            }
        }
    }



    public override Transform GetTransFromFirstAttack()
    {
        Transform targetTransfrom;
        if (listObjAttacks.Count > 0)
        {
            ObjectCtrl objectCtrl = listObjAttacks[0].GetComponent<ObjectCtrl>();
            if(objectCtrl != null)
            {
                targetTransfrom = objectCtrl.TargetPosition;
            }
            else
            {
                targetTransfrom = listObjAttacks[0].Find("TargetPosition"); // Apply Castle
            }
        }
        else if(this.enemyCtrl.TargetSkillScript.listSkillCtrl.Count>0)
        {
            targetTransfrom = this.enemyCtrl.TargetSkillScript.listSkillCtrl[0].transform;

            Debug.Log("Get Target Skill Position");
        }
        else
        {
            targetTransfrom = null;
        }

        if (targetTransfrom != null)
            return targetTransfrom;
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
