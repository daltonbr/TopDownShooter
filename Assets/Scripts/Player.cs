using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    [Header("Invencibility")]
    public bool isInvencible = false;
    [Range(0f, 5f)]
    public float invencibilityCoolDown = 1.5f;

    public bool isAIControlled = true;
    public float moveSpeed = 5f;
    public Crosshairs crosshairs;
    public float thresholdCursorDistanceSquared = 1f;
    Camera viewCamera;
    PlayerController controller;
	public GunController gunController;
    public int startingHealthPacks = 2;
    public int currentHealthPacks { get; private set; }
    [HideInInspector]
    public Vector3 spawnPoint;

    public event System.Action<int> OnChangeHPValue;

    private float dyingYValue = -10f;
    private NavMeshAgent agent;

    //private int Xbox_One_Controller = 0;
    //private int PS4_Controller = 0;

    public override void Start()
	{
		base.Start();
        if (OnChangeHPValue != null) { OnChangeHPValue(currentHealthPacks); }
    }

    void Awake()
    {
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>() as GunController;
        viewCamera = Camera.main;
        FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
        currentHealthPacks = startingHealthPacks;
        spawnPoint = this.transform.position;
        agent = this.GetComponent<NavMeshAgent>();
        Assert.IsNotNull(agent, "[Player] NavMeshAgent couldn't be found");

        if (isAIControlled)
        {    
            agent.speed = this.moveSpeed;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvencible)
        {
            health -= damage;
            if (health >= 0 && !dead)
            {
                StartCoroutine(InvencibilityTimer(invencibilityCoolDown));
            }
        }

        if (health <= 0 && !dead)
        {
            Die();
        }

    }

    IEnumerator InvencibilityTimer(float coolDownInSecs)
    {
        //Debug.Log("Waiting " + coolDownInSecs + " | Time: " + Time.time);
        isInvencible = true;
        yield return new WaitForSeconds (coolDownInSecs);
        //Debug.Log("Done | Time: " + Time.time);
        isInvencible = false;
    }
    //TODO: add a blinking effect here!

    public void AddHealthPack()
    {
        this.currentHealthPacks++;
        if (OnChangeHPValue != null) { OnChangeHPValue(currentHealthPacks); }
    }

    public void UseHealthPack()
    {
        if (currentHealthPacks > 0)
        {
            if (startingHealth != health)
            {
                currentHealthPacks--;
                if (OnChangeHPValue != null) { OnChangeHPValue(currentHealthPacks); }
                RefillHealth();
            } else
            {
                Debug.Log("Health already full!");
            }
            
        } else
        {
            Debug.Log("No more HP's left!");
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
        //TODO: [optmize] remove this check for Update
        /* kill the player when he is falling down */
        if (transform.position.y < dyingYValue)
        {
            this.Die();
        }

        // Movement Input
        Vector3 moveInput;
        Vector3 moveVelocity;

        /* Not AI controlled ? */
        if (isAIControlled)
        {
            controller.MoveAgent();
        } else
        {
            moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            moveVelocity = moveInput.normalized * moveSpeed;
            controller.Move(moveVelocity);

            // Look Input
            Assert.IsNotNull(viewCamera, "Can't find a main camera");
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
            
            /* Human Inputs */ 
            // Mouse pressed
            if (Input.GetMouseButton(0) || Input.GetButtonDown("Fire1"))
            {
                gunController.OnTriggerHold();
            }
            // Mouse released
            if (Input.GetMouseButtonUp(0) || Input.GetButtonUp("Fire1"))
            {
                gunController.OnTriggerRelease();
            }
            // Reload
            if (Input.GetKeyDown(KeyCode.R) || Input.GetButton("Fire2"))
            {
                gunController.Reload();
            }
            // HP
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButton("Fire3"))
            {
                Debug.Log("Trying to use HP");
                this.UseHealthPack();
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

    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("PlayerDeath", this.transform.position);
        base.Die();
    }

    public bool HasAnyBulletInMagazine()
    {
        return this.gunController ? gunController.HasAnyBulletInMagazine() : false;
    }

    public bool HasAnyMagazines()
    {
        return this.gunController ? gunController.HasAnyMagazines() : false;
    }

    public void Shoot()
    {
        this.gunController.OnTriggerHold();
        this.gunController.OnTriggerRelease();
    }

    public void AimCrossHairAt(Vector3 targetPosition)
    {
        this.crosshairs.transform.position = targetPosition;
    }

    public void AimAndShoot(float coolDownToShoot)
    {
        StartCoroutine(CoroutineAimAndShoot(coolDownToShoot));
    }

    private IEnumerator CoroutineAimAndShoot(float coolDownToShoot)
    {
        AimCrossHairAt(this.targetEntity.transform.position);
        yield return new WaitForSeconds(coolDownToShoot);
        Shoot();
    }

}
