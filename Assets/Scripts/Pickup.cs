using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Pickup : MonoBehaviour {

    public float rotateSpeed = 50f; // Degree per second
    public ParticleSystem pickupEffect;

    public virtual event System.Action OnCollected;

    public virtual void Awake()
    {
        Assert.IsNotNull(pickupEffect, "[Pickup] pickupEffect is null!");
    }

    void FixedUpdate()
    {
        this.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    
    //        Player player = other.GetComponent<Player>();
    //        if (!player.hasFullHealth())
    //        {
    //            player.RefillHealth();
    //            if (OnCollected != null)
    //            {
    //                AudioManager.instance.PlaySound("PickupHealth", this.transform.position);
    //                Destroy(Instantiate(hpEffect.gameObject, this.transform.position, Quaternion.Euler(new Vector3(90, 0, 0))), hpEffect.main.duration);
    //                OnCollected();
    //            }
    //            Destroy(this.gameObject);
    //        }
    //        //			Debug.Log("Full Health");
    //    }

    //}
}
