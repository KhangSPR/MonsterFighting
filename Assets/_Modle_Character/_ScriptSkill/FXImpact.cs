using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class FXImpact : SkillAbstract
{
    [Header("FX Impact")]
    [SerializeField] protected Rigidbody2D _rigidbody;

    // Set to store collided ObjectCtrl objects
    private HashSet<ObjectCtrl> collidedObjectsEnemy = new HashSet<ObjectCtrl>();
    private HashSet<ObjectCtrl> collidedObjectsPlayer = new HashSet<ObjectCtrl>();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRigidbody2D();
    }
    protected virtual void LoadRigidbody2D()
    {
        if (this._rigidbody != null) return;
        this._rigidbody = GetComponent<Rigidbody2D>();
        this._rigidbody.bodyType = RigidbodyType2D.Kinematic;
        Debug.Log(transform.name + ": LoadRigidbody", gameObject);
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.skillCtrl.ObjectCtrl == null) return;

        if (collision.name == "CanAttack" || collision.name == "ObjMelee") return;

        // Check if the parent of the collider is tagged "Enemy"
        if (this.skillCtrl.ObjectCtrl != null)
        {
            if (this.skillCtrl.ObjectCtrl.transform.CompareTag("Player"))
            {
                if (collision.transform.parent.CompareTag("Enemy"))
                {
                    // Get ObjectCtrl from the parent object
                    ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

                    // If ObjectCtrl hasn't been collided before, process the impact
                    if (!collidedObjectsEnemy.Contains(objectCtrl))
                    {
                        // Add ObjectCtrl to the collided objects list
                        collidedObjectsEnemy.Add(objectCtrl);

                        Debug.Log("Ontrigger SkillCollider + " + transform.parent.name);

                        // Call SkillCollider and send damage
                        this.skillCtrl.SkillColider(objectCtrl);
                    }
                }
            }
            else if (this.skillCtrl.ObjectCtrl.transform.CompareTag("Enemy"))
            {
                if (collision.transform.parent.CompareTag("Castle") || collision.transform.parent.CompareTag("Player"))
                {
                    // Get ObjectCtrl from the parent object
                    ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

                    // If ObjectCtrl hasn't been collided before, process the impact
                    if (!collidedObjectsPlayer.Contains(objectCtrl))
                    {
                        // Add ObjectCtrl to the collided objects list
                        collidedObjectsPlayer.Add(objectCtrl);

                        Debug.Log("Ontrigger SkillCollider + " + transform.parent.name);

                        // Call SkillCollider and send damage
                        this.skillCtrl.SkillColider(objectCtrl);
                    }
                }
            }

        }
    }

    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (this.skillCtrl.ObjectCtrl.transform.CompareTag("Player"))
        {
            if (collision.transform.parent.CompareTag("Enemy"))
            {
                ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

                // Remove ObjectCtrl from the list when exiting collision
                if (collidedObjectsEnemy.Contains(objectCtrl))
                {
                    collidedObjectsEnemy.Remove(objectCtrl);
                }
            }
            else if (this.skillCtrl.ObjectCtrl.transform.CompareTag("Enemy"))
            {
                if (collision.transform.parent.CompareTag("Castle") || collision.transform.parent.CompareTag("Player"))
                {
                    // Get ObjectCtrl from the parent object
                    ObjectCtrl objectCtrl = collision.transform.parent.GetComponent<ObjectCtrl>();

                    // Remove ObjectCtrl from the list when exiting collision
                    if (collidedObjectsPlayer.Contains(objectCtrl))
                    {
                        collidedObjectsPlayer.Remove(objectCtrl);
                    }
                }

            }
        }
    }
    public void ClearCollider()
    {
        collidedObjectsEnemy.Clear();
        collidedObjectsPlayer.Clear();
    }
}