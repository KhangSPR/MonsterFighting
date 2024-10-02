using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAbstract : SaiMonoBehaviour
{
    [SerializeField] protected SkillCtrl skillCtrl;
    public SkillCtrl SkillCtrl => skillCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadSkillCtrl();
    }
    public void LoadSkillCtrl()
    {
        if (skillCtrl != null) return;
        skillCtrl = transform.parent.GetComponent<SkillCtrl>();
        Debug.Log(transform.name + ": LoadSkillCtrl", gameObject);
    }

}
