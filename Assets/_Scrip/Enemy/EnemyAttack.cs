using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : EnemyAbstract
{
    public bool canAttack = false;

    public List<Transform> CanAtacck = new List<Transform>();
    private bool detectedFirstCollision = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.tag == "Player" || other.transform.parent.tag == "Castle")
        {
            if (!detectedFirstCollision && CanAtacck.Count == 0)
            {
                canAttack = true;
                detectedFirstCollision = true;
                CanAtacck.Add(other.transform.parent);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CanAtacck.Contains(other.transform.parent))
        {
            CanAtacck.Remove(other.transform.parent);

            if (CanAtacck.Count == 0)
            {
                canAttack = false;
                detectedFirstCollision = false;
            }
        }
    }
    public Transform GetTransFromFirstAttack()
    {
        return CanAtacck[0];
    }
}
