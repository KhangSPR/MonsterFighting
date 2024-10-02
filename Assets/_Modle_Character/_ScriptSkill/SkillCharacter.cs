using System;

[Serializable]
public class SkillCharacter
{
    public float manaSkill;
    public bool unlockSkill;
    public ISkill skillType;
    public float damage;

    public SkillCharacter(float mana, bool unlock, float damage, ISkill skillType)
    {
        manaSkill = mana;
        unlockSkill = unlock;
        this.skillType = skillType;
        this.damage = damage;
    }

    public bool CanUseSkill(ObjMana objMana)
    {
        return unlockSkill && objMana.IsMana >= manaSkill;
    }
    public void ActiveSkill(ObjectCtrl objectCtrl)
    {
        //Skill Action 
        skillType.ExecuteSkill(objectCtrl, damage);
    }
    public void UseSkill(ObjectCtrl objectCtrl)
    {
        if (CanUseSkill(objectCtrl.ObjMana))
        {
            //Deduct Mana
            objectCtrl.ObjMana.DeductMana(manaSkill);
        }
    }

}