using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GunPickup : Pickup
{

    public Gun gunToEquip;
    public override event System.Action OnCollected;
    
    public override void Awake()
    {
        base.Awake();
        Assert.IsNotNull(gunToEquip, "[GunPickup] Gun can't be null!");
    }

    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            //Debug.Log(this.name + " entered by " + other.name);
            
            Player playerScript = other.GetComponent<Player>();
            if (playerScript)
            {
                playerScript.gunController.EquipGun(gunToEquip);
            }

            //TODO: why I can't access this script?
            //GunController playerGunController = other.GetComponent<GunController>();
            //if (!playerGunController)
            //{
            //    playerGunController.EquipGun(gunToEquip);
            //} else
            //{
            //    Debug.LogWarning("[GunPickup] GunController wasn't found on player!");
            //}

            AudioManager.instance.PlaySound("PickupGun", this.transform.position);
            Destroy(Instantiate(pickupEffect.gameObject, this.transform.position, Quaternion.identity), pickupEffect.main.duration);
            Destroy(this.gameObject);
        }

    }
}
