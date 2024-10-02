using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FXImpact : SkillAbstract
{
    [Header("FX Impact")]
    //[SerializeField] protected CircleCollider2D circleCollider;
    [SerializeField] protected Rigidbody2D _rigidbody;

    // Set to store collided ObjectCtrl objects
    private HashSet<ObjectCtrl> collidedObjects = new HashSet<ObjectCtrl>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        //this.LoadCircleCollider2D();
        this.LoadRigidbody2D();
    }

    //protected virtual void LoadCircleCollider2D()
    //{
    //    if (this.circleCollider != null) return;
    //    this.circleCollider = GetComponent<CircleCollider2D>();
    //    this.circleCollider.isTrigger = true;
    //    Debug.Log(transform.name + ": LoadCollider", gameObject);
    //}

    protected virtual void LoadRigidbody2D()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log(transform.name + ": LoadRigidbody", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the parent of the collider is tagged "Enemy"
        if (collision.transform.parent.CompareTag("Enemy"))
        {
            // Get ObjectCtrl from the parent object
            ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

            // If ObjectCtrl hasn't been collided before, process the impact
            if (!collidedObjects.Contains(objectCtrl))
            {
                // Add ObjectCtrl to the collided objects list
                collidedObjects.Add(objectCtrl);

                Debug.Log("Ontrigger SkillCollider + " + transform.parent.name);

                // Call SkillCollider and send damage
                this.skillCtrl.SkillColider(objectCtrl);
            }
        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the parent of the collider is tagged "Enemy"
        if (collision.transform.parent.CompareTag("Enemy"))
        {
            // Get ObjectCtrl from the parent object
            ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

            // Remove ObjectCtrl from the list when exiting collision
            if (collidedObjects.Contains(objectCtrl))
            {
                collidedObjects.Remove(objectCtrl);
            }
        }
    }
}