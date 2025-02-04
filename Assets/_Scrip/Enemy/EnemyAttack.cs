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
                if(skillCtrl is VirtualShieldCtrl virtualShieldCtrl)
                {
                    virtualShieldCtrl.enemyCtrls.Add(enemyCtrl);

                }
                Debug.Log("Add Skill");
            }

            Debug.Log("Trigger Skill");
            return;
        }

        if (other.transform.parent.tag == "Player" && other.transform.name == "Modle")
        {
            ObjectCtrl objectCtrl = other.transform.parent.GetComponent<ObjectCtrl>();

            if (!objectCtrl.ObjLand.CampareLand(objCtrl.ObjLand.LandIndex))
                return;

            if (objectCtrl.ObjectDamageReceiver.IsDead)
            {
                return;
            }

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
                    //this.enemyCtrl.TargetSkillScript.listSkillCtrl.Remove(skillCtrl);

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
        // Nếu danh sách kỹ năng của đối tượng mục tiêu không rỗng
        if (this.enemyCtrl.TargetSkillScript.listSkillCtrl.Count > 0)
        {
            foreach (SkillCtrl skillCtrl in this.enemyCtrl.TargetSkillScript.listSkillCtrl)
            {
                // Kiểm tra xem kỹ năng có phải là MagicVortexCtrl không
                if (skillCtrl is MagicVortexCtrl)
                {
                    return skillCtrl.transform;
                }
            }
            // Nếu không có MagicVortexCtrl, trả về kỹ năng đầu tiên
            return this.enemyCtrl.TargetSkillScript.listSkillCtrl[0].transform;
        }

        // Nếu danh sách tấn công không rỗng
        if (listObjAttacks.Count > 0)
        {
            Transform targetTransform = null;

            // Lấy ObjectCtrl từ đối tượng đầu tiên
            ObjectCtrl objectCtrl = listObjAttacks[0].GetComponent<ObjectCtrl>();
            if (objectCtrl != null)
            {
                targetTransform = objectCtrl.TargetPosition;
            }
            else
            {
                // Nếu không có ObjectCtrl, tìm Transform "TargetPosition"
                targetTransform = listObjAttacks[0].Find("TargetPosition");
            }

            return targetTransform;
        }

        // Trường hợp không tìm thấy mục tiêu
        return null;
    }

    public void OnDeadCastle(bool isRange)
    {

        //Obj Move In The City 
        this.enemyCtrl.ObjMovement.MoveSpeed = 0f;
        this.listObjAttacks.Clear();
        this.checkCanAttack = false;
        this.enemyCtrl.ObjMoveIntheCity.IsMoveInTheCity = isRange;

        Debug.Log("OnDeadCastle");
    }
}
