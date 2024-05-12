using UnityEngine;

public abstract class DeFenSeAbstract : SaiMonoBehaviour
{
    [Header("Guard Abtract")]
    [SerializeField] protected DeFenSeCtrl deFenSeCtrl;
    public DeFenSeCtrl DeFenSeCtrl { get => deFenSeCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadDeFenSeCtrl();
    }

    protected virtual void LoadDeFenSeCtrl()
    {
        if (this.deFenSeCtrl != null) return;
        this.deFenSeCtrl = transform.parent.GetComponent<DeFenSeCtrl>();
        Debug.Log(transform.name + ": LoadDeFenSeCtrl", gameObject);
    }
}
