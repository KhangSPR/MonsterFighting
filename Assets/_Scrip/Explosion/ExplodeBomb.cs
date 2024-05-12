using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using EZCameraShake;

public class ExplodeBomb : MonoBehaviour
{
    public float delay = 3f;
    public float force = 700f;
    public float radius = 5f;

    public GameObject explosionEffect;
    public GameObject BreackEffect;

    public LayerMask LayerToHit;

    float countDown;
    bool hasExploded = false;

    private void Start()
    {
        countDown = delay;
    }
    private void Update()
    {
        countDown -= Time.deltaTime;
        if(countDown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }
    void Explode()
    {
        

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, radius, LayerToHit);

        foreach(Collider2D obj in objects)
        {
            Vector2 direction = obj.transform.position - transform.position;
            
            obj.GetComponent<Rigidbody2D>().AddForce(direction * force);
        }

        Transform NewBreack = FXSpawner.Instance.Spawn(FXSpawner.BreackOne, transform.position, transform.rotation);
        if (NewBreack == null) return;
        NewBreack.gameObject.SetActive(true);

        CameraShaker.Instance.ShakeOnce(4, 4, 0.1f, 1f);

        Transform Newexplosion = FXSpawner.Instance.Spawn(FXSpawner.ImpactOne, transform.position, transform.rotation);
        if (Newexplosion == null) return;
        Newexplosion.gameObject.SetActive(true);

        Destroy(gameObject);


    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
