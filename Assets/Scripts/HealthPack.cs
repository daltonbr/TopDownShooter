using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthPack : MonoBehaviour {

	//public GameObject healthPack;
	public float rotateSpeed = 50f;	// Degree per second
	public float recoverPercentage = 1f;

	public event System.Action OnCollected;

	void Awake () {
		
	}

	void FixedUpdate () {
		this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log("Healthpack entered by " + other.name);
		if (other.tag == "Player")
		{
//			Debug.Log("Healthpack entered by " + other.name);
			Player player = other.GetComponent<Player>();
			if (!player.hasFullHealth())
			{
				player.RefillHealth();
				if (OnCollected != null)
				{
					OnCollected();
				}
				Destroy(this.gameObject);
			}
//			Debug.Log("Full Health");
		}

	}
}
