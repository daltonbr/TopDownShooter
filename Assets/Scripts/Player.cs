using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
//[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public bool isInvencible = false;
    public float moveSpeed = 5f;
    public Crosshairs crosshairs;
    public float thresholdCursorDistanceSquared = 1f;
    Camera viewCamera;
    PlayerController controller;
	public GunController gunController;

    //private int Xbox_One_Controller = 0;
    //private int PS4_Controller = 0;

    public override void Start()
	{
		base.Start();
    }

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>() as GunController;
        viewCamera = Camera.main;
        FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
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

    void OnNewWave(int waveNumber)
    {
        health = startingHealth;
        //TODO: check if this gun index really exist
        gunController.EquipGun(waveNumber - 1);
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
		Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
		float rayDistance;

		if (groundPlane.Raycast(ray, out rayDistance))
		{
			Vector3 point = ray.GetPoint(rayDistance);
			//Debug.DrawLine(ray.origin, point, Color.red);
			controller.LookAt(point);
            crosshairs.transform.position = point;
            crosshairs.DetectTargets(ray);
            // Tweak for the gun not twist when following the cursor (sqrMagnitude is faster)
            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > thresholdCursorDistanceSquared)
            {
                gunController.Aim(point);
            }
            
            
		}

    	// Weapon Input
        // Mouse pressed
		if (Input.GetMouseButton(0) || Input.GetButtonDown("Fire1"))
		{
			gunController.OnTriggerHold();
		}
        // Mouse release
        if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1"))
        {
            gunController.OnTriggerRelease();
        }
        if (Input.GetKeyDown(KeyCode.R)|| Input.GetButton("Fire2"))
        {
            gunController.Reload();
        }

        if (transform.position.y < -10)
        {
            TakeDamage(health);
        }

        // Xbox1 and PS4 controller detection

        //string[] names = Input.GetJoystickNames();
        //for (int x = 0; x < names.Length; x++)
        //{
        //    print(names[x].Length);
        //    if (names[x].Length == 19)
        //    {
        //        print("PS4 CONTROLLER IS CONNECTED");
        //        PS4_Controller = 1;
        //        Xbox_One_Controller = 0;
        //    }
        //    if (names[x].Length == 33)
        //    {
        //        print("XBOX ONE CONTROLLER IS CONNECTED");
        //        //set a controller bool to true
        //        PS4_Controller = 0;
        //        Xbox_One_Controller = 1;

        //    }
        //}


        //if (Xbox_One_Controller == 1)
        //{
        //    //do something
        //    Debug.Log("Xbox");
        //}
        //else if (PS4_Controller == 1)
        //{
        //    //do something
        //    Debug.Log("PS4");
        //}
        //else
        //{
        //    Debug.Log("No Controller");
        //    // there is no controllers
        //}


    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("PlayerDeath", this.transform.position);
        base.Die();
    }

}
