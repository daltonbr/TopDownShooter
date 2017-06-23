using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthPack : Pickup {

    public override event System.Action OnCollected;
    
    void OnTriggerStay(Collider other) {
		
		if (other.tag == "Player")
		{
            Player player = other.GetComponent<Player>();
            player.AddHealthPack();
            AudioManager.instance.PlaySound("PickupHealth", this.transform.position);
            Destroy(Instantiate(pickupEffect.gameObject, this.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), pickupEffect.main.duration);

            if (OnCollected != null)
			{           
				OnCollected();
			}
			Destroy(this.gameObject);
		
		}

	}
}
