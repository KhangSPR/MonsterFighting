using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : PlayerAbstract
{
    public bool canAttack = false;
    public List<Transform> CanAttack = new List<Transform>();
    private bool detectedFirstCollision = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent != null && other.transform.parent.tag == "Enemy")
        {
            if (!detectedFirstCollision)
            {
                canAttack = true;
                detectedFirstCollision = true;
            }
            if (!CanAttack.Contains(other.transform.parent))
            {
                CanAttack.Add(other.transform.parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.parent != null && CanAttack.Contains(other.transform.parent))
        {
            CanAttack.Remove(other.transform.parent);
            if (CanAttack.Count == 0)
            {
                canAttack = false;
                detectedFirstCollision = false;
                RecheckColliders();  // Recheck the colliders to ensure no enemy is still within range
            }
        }
    }

    private void RecheckColliders()
    {
        foreach (Transform enemy in CanAttack)
        {
            if (enemy != null && enemy.tag == "Enemy")
            {
                canAttack = true;
                detectedFirstCollision = true;
                return;  // As soon as one valid enemy is found, set canAttack to true and exit
            }
        }
    }

    public Transform GetTransFromFirstAttack()
    {
        if (CanAttack.Count > 0)
            return CanAttack[0];
        return null;
    }
}
