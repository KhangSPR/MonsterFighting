using System.Collections.Generic;
using UnityEngine;

public class ParticleCtrl : SaiMonoBehaviour
{
    [SerializeField] protected ParticleDameSender _sender;
    public ParticleDameSender particleDamesender => _sender;
    [SerializeField] protected ParticleImpact _impact;
    public ParticleImpact Impact => _impact;
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadParticleDameSender();
        this.LoadParticleImpact();
    }

    protected virtual void LoadParticleDameSender()
    {
        if (_sender != null) return;
        _sender = transform.GetComponentInChildren<ParticleDameSender>();
        Debug.Log(gameObject.name + ": LoadParticleDameSender" + gameObject);
    }

    protected virtual void LoadParticleImpact()
    {
        if (_impact != null) return;
        _impact = transform.GetComponentInChildren<ParticleImpact>();
        Debug.Log(gameObject.name + ": LoadParticleImpact" + gameObject);
    }
}
