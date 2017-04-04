using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public bool isInvencible = false;
    public float moveSpeed = 5f;
	Camera viewCamera;
    PlayerController controller;
	GunController gunController;

    public override void Start()
	{
		base.Start();
        controller = GetComponent<PlayerController>();
		gunController = GetComponent<GunController>();
		viewCamera = Camera.main;
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvencible)
        {
            health -= damage;
        }

        if (health <= 0 && !dead)
        {
            Die();
        }
    }

    void Update()
    {
		// Movement Input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

		// Look Input
		Assert.IsNotNull(viewCamera,"Can't find a main camera");
		Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
		Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
		float rayDistance;

		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin, point, Color.red);
			controller.LookAt(point);
		}

    	// Weapon Input
        // Mouse pressed
		if (Input.GetMouseButton(0))
		{
			gunController.OnTriggerHold();
		}
        // Mouse release
        if (Input.GetMouseButtonUp(0))
        {
            gunController.OnTriggerRelease();
        }
    }
}
