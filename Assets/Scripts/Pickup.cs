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

}
