using System;
using UnityEngine; // Sử dụng UnityEngine cho Vector3

[Serializable]
public class SkillCharacter
{
    public float manaSkill;
    public bool unlockSkill;
    public ISkill skillType;
    public float damage;
    public float distanceAttack;

    public SkillCharacter(float mana, bool unlock, float damage, ISkill skillType, float distanceAttack)
    {
        manaSkill = mana;
        unlockSkill = unlock;
        this.skillType = skillType;
        this.damage = damage;
        this.distanceAttack = distanceAttack;
    }

    // Thêm tham số vị trí hiện tại và vị trí mục tiêu để kiểm tra khoảng cách
    public bool CanUseSkill(ObjMana objMana, Vector3 currentPosition, Vector3 targetPosition)
    {
        // Tính khoảng cách giữa vị trí hiện tại và mục tiêu
        float distance = Vector3.Distance(currentPosition, targetPosition);

        // Log ra khoảng cách đã tính được
        //Debug.Log("Distance to target: " + distance);

        // Kiểm tra nếu skill đã mở khóa, đủ mana và khoảng cách trong giới hạn
        return unlockSkill && objMana.IsMana >= manaSkill && Vector3.Distance(currentPosition, targetPosition) >= distanceAttack;
    }

    public void ActiveSkill(ObjectCtrl objectCtrl)
    {
        // Kích hoạt hành động skill
        skillType.ExecuteSkill(objectCtrl, damage);
    }

    public void UseSkill(ObjectCtrl objectCtrl)
    {
        // Trừ mana
        objectCtrl.ObjMana.DeductMana(manaSkill);
        // Kích hoạt skill
        //ActiveSkill(objectCtrl);
    }
}
