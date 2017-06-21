using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthPack : Pickup {

    public override event System.Action OnCollected;
    //public float recoverPercentage = 1f;

    void OnTriggerStay(Collider other) {
		
		if (other.tag == "Player")
		{
            Debug.Log("Pickup " + this.name + " entered by " + other.name);
			Player player = other.GetComponent<Player>();
			if (!player.hasFullHealth())
			{
				player.RefillHealth();
				if (OnCollected != null)
				{
                    AudioManager.instance.PlaySound("PickupHealth", this.transform.position);
                    Destroy(Instantiate(pickupEffect.gameObject, this.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), pickupEffect.main.duration);
					OnCollected();
				}
				Destroy(this.gameObject);
			}
            //Debug.Log("Full Health");
		}

	}
}
