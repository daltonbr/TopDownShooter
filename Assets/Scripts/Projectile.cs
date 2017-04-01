using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	float speed = 10f;

	public void SetSpeed(float newSpeed)
	{
		this.speed = newSpeed;
	}
		
	void Update ()
	{
		this.transform.Translate (Vector3.forward * Time.deltaTime * speed);
	}
}
