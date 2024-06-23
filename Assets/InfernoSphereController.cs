using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfernoSphereController : MonoBehaviour
{
    public int damagePerSecond_1;
    public int damagePerSecond_2;
    public int damagePerSecond_3;

    [Range(1, 3)] public int currentSphereLevel = 1;
    private float damageInterval = 0.5f;

    private bool canAttack = false;
    private Coroutine damageCoroutine;

    public void UpSphereLevel()
    {
        if (currentSphereLevel < 3)
        {
            currentSphereLevel++;
        }
    }

    public void DownSphereLevel()
    {
        if (currentSphereLevel > 1)
        {
            currentSphereLevel--;
        }
    }

    int GetCurrentSphereDamageLevel()
    {
        switch (currentSphereLevel)
        {
            case 2: return damagePerSecond_2;
            case 3: return damagePerSecond_3;
            case 1:
            default: return damagePerSecond_1;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Enemy"))
        {
            Debug.Log("Is Enemy : " + collision.transform.parent.name);
            canAttack = true;

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DamagePerSecond(collision));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.CompareTag("Enemy"))
        {
            canAttack = false;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DamagePerSecond(Collider2D collision)
    {
        while (canAttack)
        {
            var damageReceiver = collision.transform.parent.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver != null)
            {
                Debug.Log("Found Receiver : " + damageReceiver.transform.parent.name);
                var damagePerSecond = GetCurrentSphereDamageLevel();
                damageReceiver.deDuctHP(damagePerSecond, true);
            }
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
