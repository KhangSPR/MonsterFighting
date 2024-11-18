using UnityEngine;

[RequireComponent(typeof(Explodable))]
public class ExplodableTrigger : MonoBehaviour {

	private Explodable _explodable;

	void Start()
	{
		_explodable = GetComponent<Explodable>();
	}
	public void Trigger()
	{
		_explodable.explode();
		ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
		ef.doExplosion(transform.position);
	}
}
