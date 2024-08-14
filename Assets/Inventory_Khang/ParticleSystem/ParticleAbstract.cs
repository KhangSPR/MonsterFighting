using UnityEngine;

public abstract class ParticleAbstract : SaiMonoBehaviour
{
    [Header("ParticleCtrl Abtract")]
    [SerializeField] protected ParticleCtrl particleCtrl;
    public ParticleCtrl ParticleCtrl { get => particleCtrl; }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadParticleCtrl();
    }

    protected virtual void LoadParticleCtrl()
    {
        if (this.particleCtrl != null) return;
        this.particleCtrl = transform.parent.GetComponent<ParticleCtrl>();
        Debug.Log(transform.name + ": LoadBulletCtrl", gameObject);
    }
}
