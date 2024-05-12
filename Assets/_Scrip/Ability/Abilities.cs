using UnityEngine;

public class Abilities : SaiMonoBehaviour
{
    [Header("Ability ObjectCtrl")]
    [SerializeField] protected PortalCtrl portalCtrl;
    public PortalCtrl PortalCtrl => portalCtrl;
    [Header("Abilities")]
    [SerializeField] protected AbilitySummonEnemy abilitySummonEnemy;
    public AbilitySummonEnemy AbilitySummonEnemy => abilitySummonEnemy;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadPortalCtrl();
        this.LoadAbilitySummonEnemy();
    }
    protected virtual void LoadPortalCtrl()
    {
        if (this.portalCtrl != null) return;
        this.portalCtrl = transform.parent.GetComponent<PortalCtrl>();
        Debug.Log(gameObject.name + ": LoadPortalCtrl" + gameObject);
    }
    protected virtual void LoadAbilitySummonEnemy()
    {
        if (this.abilitySummonEnemy != null) return;
        this.abilitySummonEnemy = transform.GetComponentInChildren<AbilitySummonEnemy>();
        Debug.Log(gameObject.name + ": loadAbilitySummonEnemy" + gameObject);
    }
}
