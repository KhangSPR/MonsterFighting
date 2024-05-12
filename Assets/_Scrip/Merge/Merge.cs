using System.Collections;
using UnityEngine;

public class Merge : MonoBehaviour
{
    public GameObject MergedObject;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MergeBlock"))
        {
            StartCoroutine(DelayedMerge(other));
        }
    }

    IEnumerator DelayedMerge(Collider2D other)
    {
        yield return new WaitForSeconds(1.0f);

        Vector2 collisionPoint = (transform.position + other.transform.position) / 2f;  // Calculate the midpoint
        Debug.Log("Hit Detected");

        // Instantiate the merged object after the delay
        GameObject mergedObject = Instantiate(MergedObject, collisionPoint, Quaternion.identity) as GameObject;

        // Destroy the colliding objects
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
