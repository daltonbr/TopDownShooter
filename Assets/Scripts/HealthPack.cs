using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthPack : MonoBehaviour {

	//public GameObject healthPack;
	public float rotateSpeed = 50f;	// Degree per second
	public float recoverPercentage = 1f;
    public ParticleSystem hpEffect;

	public event System.Action OnCollected;

    public void Awake()
    {
        Assert.IsNotNull(hpEffect, "[HealthPack] hpEffect can't be null!");
    }


    void FixedUpdate () {
		this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}

	void OnTriggerStay(Collider other) {
		//Debug.Log("Healthpack in contact with " + other.name);
		if (other.tag == "Player")
		{
//			Debug.Log("Healthpack entered by " + other.name);
			Player player = other.GetComponent<Player>();
			if (!player.hasFullHealth())
			{
				player.RefillHealth();
				if (OnCollected != null)
				{
                    AudioManager.instance.PlaySound("PickupHealth", this.transform.position);
                    Destroy(Instantiate(hpEffect.gameObject, this.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), hpEffect.main.duration);
					OnCollected();
				}
				Destroy(this.gameObject);
			}
//			Debug.Log("Full Health");
		}

	}
}
