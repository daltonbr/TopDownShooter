using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidbody;
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

	void FixedUpdate()
	{
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
	}

    public void Move(Vector3 _velocity)
    {
        this.velocity = _velocity;
    }
}
