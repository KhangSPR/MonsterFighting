/*This script created by using docs.unity3d.com/ScriptReference/MonoBehaviour.OnParticleCollision.html*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticleCollisionInstance : MonoBehaviour
{
    public GameObject[] EffectsOnCollision;
    public float DestroyTimeDelay = 5;
    public bool UseWorldSpacePosition;
    public float Offset = 0;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public bool useOnlyRotationOffset = true;
    public bool UseFirePointRotation;
    public bool DestoyMainEffect = true;
    private ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    private ParticleSystem ps;

    // HashSet to track particle indices that have already triggered the effect
    private HashSet<int> triggeredParticles = new HashSet<int>();

    [SerializeField] ArrowRainCtrl arrowRainCtrl;
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    void LateUpdate()
    {
        // Check the particle velocities every frame
        CheckParticleSpeed();
    }

    void CheckParticleSpeed()
    {
        var particles = new ParticleSystem.Particle[part.particleCount];
        part.GetParticles(particles);

        foreach (var particle in particles)
        {   
            //Debug.Log(particle.velocity.magnitude);
            // Calculate the speed of the particle
            if (particle.velocity.magnitude < 0.1f && !triggeredParticles.Contains(particle.GetHashCode()))
            {   
                triggeredParticles.Add(particle.GetHashCode());
                HandleZeroSpeedParticle(particle);
            }
        }
    }

    void HandleZeroSpeedParticle(ParticleSystem.Particle particle)
    {
        // Your custom logic for zero-speed particles, e.g. trigger effect
        Vector3 position = part.transform.TransformPoint(particle.position);
        foreach (var effect in EffectsOnCollision)
        {
            var instance = Instantiate(effect, position + Vector3.up * Offset, Quaternion.identity) as GameObject;
            // if (!UseWorldSpacePosition) instance.transform.parent = transform;
            // if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
            // else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
            // else
            // {
            //     instance.transform.LookAt(position);
            //     instance.transform.rotation *= Quaternion.Euler(rotationOffset);
            // }
            ArrowRainHit arrowRainHit = instance.GetComponent<ArrowRainHit>();
            if (arrowRainHit == null || arrowRainCtrl == null)
            {
                Debug.LogError("arrowRainHit == null || arrowRainCtrl == null");
                return;
            }
            arrowRainHit.DamageSender.Damage = arrowRainCtrl.damageHit;

            Destroy(instance, DestroyTimeDelay);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            foreach (var effect in EffectsOnCollision)
            {
                var instance = Instantiate(effect, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
                if (!UseWorldSpacePosition) instance.transform.parent = transform;
                if (UseFirePointRotation) { instance.transform.LookAt(transform.position); }
                else if (rotationOffset != Vector3.zero && useOnlyRotationOffset) { instance.transform.rotation = Quaternion.Euler(rotationOffset); }
                else
                {
                    instance.transform.LookAt(collisionEvents[i].intersection + collisionEvents[i].normal);
                    instance.transform.rotation *= Quaternion.Euler(rotationOffset);
                }
                Destroy(instance, DestroyTimeDelay);
            }
        }
        if (DestoyMainEffect == true)
        {
            Destroy(gameObject, DestroyTimeDelay + 0.5f);
        }
    }
}
