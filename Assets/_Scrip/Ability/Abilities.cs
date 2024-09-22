using UnityEngine;

public class Abilities : SaiMonoBehaviour
{
    [Header("Ability ObjectCtrl")]
    [SerializeField] protected AbilityPointAbstract abilityCtrl;
    public AbilityPointAbstract AbilityCtrl => abilityCtrl;
    [Header("Abilities")]
    [SerializeField] protected AbilitySummon abilitySummon;
    public AbilitySummon AbilitySummon => abilitySummon;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPortalCtrl();
        this.LoadAbilitySummonEnemy();
    }
    protected virtual void LoadPortalCtrl()
    {
        if (this.abilityCtrl != null) return;
        this.abilityCtrl = transform.parent.GetComponent<AbilityPointAbstract>();
        Debug.Log(gameObject.name + ": LoadPortalCtrl" + gameObject);
    }
    protected virtual void LoadAbilitySummonEnemy()
    {
        if (this.abilitySummon != null) return;
        this.abilitySummon = transform.GetComponentInChildren<AbilitySummon>();
        Debug.Log(gameObject.name + ": loadAbilitySummonEnemy" + gameObject);
    }
}
