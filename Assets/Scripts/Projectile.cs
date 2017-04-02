using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10f;
	float damage = 1f;

    float lifetime = 3f;
    float skinWidth = .1f;      // used to ensure the bullet will hit the enemy, sometimes when the enemy moves at high speeds the collision could not be detected

    void Start()
    {
        Destroy(this.gameObject, lifetime);

        Collider[] initialCollisions = Physics.OverlapSphere(this.transform.position, .1f, collisionMask);
        if (initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

	public void SetSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}
		
	void Update ()
	{
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		this.transform.Translate (Vector3.forward * moveDistance);
	}

	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(this.transform.position, this.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject(hit);
		}
	}

	void OnHitObject(RaycastHit hit)
	{
		//print(hit.collider.gameObject.name);
		IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
		if (damageableObject != null)
		{
			damageableObject.TakeHit(damage, hit);
		}
		GameObject.Destroy(this.gameObject);
	}

    void OnHitObject(Collider coll)
    {
        IDamageable damageableObject = coll.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        GameObject.Destroy(this.gameObject);
    }
}
