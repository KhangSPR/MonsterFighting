using System.Collections;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public Animator animator;
    public int damagePerSecond;
    [Min(0)] public float speed;
    private bool canAttack = false;
    private Coroutine damageCoroutine;
    private float damageInterval = 0.5f;

    private void Start()
    {
        // Any initialization if needed
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isEnemy = collision.transform.parent.tag == "Enemy";

        if (!isEnemy) return;
        Debug.Log("Is Enemy: " + isEnemy);
        canAttack = true;

        // Start dealing damage when entering the collider
        if (damageCoroutine == null)
        {
            damageCoroutine = StartCoroutine(DamagePerSecond(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        bool isEnemy = collision.transform.parent.tag == "Enemy";
        if (isEnemy)
        {
            canAttack = false;
            // Stop dealing damage when exiting the collider
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // No need to handle anything here for DPS as it's managed by the coroutine
    }

    private IEnumerator DamagePerSecond(Collider2D collision)
    {
        while (canAttack)
        {
            var damageReceiver = collision.transform.parent.GetComponentInChildren<DamageReceiver>();
            if (damageReceiver != null)
            {
                Debug.Log("Found Receiver: " + damageReceiver.transform.parent.name);
                damageReceiver.deDuctHP(damagePerSecond, true);
            }
            // Wait for one second before dealing damage again
            yield return new WaitForSeconds(damageInterval);
        }
    }
}
