using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	public LayerMask collisionMask;
	float speed = 10f;

	public void SetSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}
		
	void Update ()
	{
		float moveDistance = speed * Time.fixedDeltaTime;
		CheckCollisions (moveDistance);
		this.transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}

	void CheckCollisions(float moveDistance)
	{
		Ray ray = new Ray(this.transform.position, this.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide))
		{
			OnHitObject(hit);
		}
	}

	void OnHitObject(RaycastHit hit)
	{
		print(hit.collider.gameObject.name);
		GameObject.Destroy(this.gameObject);
	}
}
