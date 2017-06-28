using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AIManager : MonoBehaviour
{
    public Player player;
    private PlayerController playerController;
    public Context context;
    public Scanner scanner;

    /* TacticalMovement Scorerers */
    public MoveToPickup moveToPickup;
    public MoveToBestPosition moveToBestPosition;

    /* PlayerActions Scorerers */
    public UseHealth useHealth;
    public ReloadGun reloadGun;
    public SetBestAttackTarget setBestAttackTarget;

    [Header("Tweaks")]
    //[Range(0f, 10f)]
    //public float aiErrorFactor = 1f;
    [Range(0f, 3f)]
    public float coolDownToShoot = 1.5f;

    [Header("Scanner")]
    public float scanTimeIntervalInSecs = 1f;
    [Range(0f, 30f)]
    public float enemyScanRange = 10f;
    [Range(0f, 30f)]
    public float pickupScanRange = 10f;
    [Range(0.5f, 3f)]
    public float samplingDensity = 1.5f;
    [Range(3f, 30f)]
    public float samplingRange = 12f;

    [Header("Debug")]
    public bool debugMode = true;
    public GameObject debugPrefab;
    public TextMesh debugScoreText;
    [Range(0f, 1f)]
    public float transparency = 0.25f;
    [Range(0.1f, 1f)]
    public float radius = 0.5f;
    private DebugSphereManager debugSphereManager;
    GameObject debugPrefabHolder;
    //[HideInInspector]
    //public Vector3 desiredShootingPosition;


    void Awake()
    {
        /* Instantiating */
        this.player = this.gameObject.GetComponent<Player>();
        this.context = new Context(player);
        this.scanner = this.gameObject.AddComponent<Scanner>();
        this.playerController = player.GetComponent<PlayerController>();
        this.moveToPickup = new MoveToPickup();
        this.moveToBestPosition = new MoveToBestPosition();
        this.useHealth = new UseHealth();
        this.reloadGun = new ReloadGun();
        this.setBestAttackTarget = new SetBestAttackTarget();
        this.debugSphereManager = this.gameObject.AddComponent<DebugSphereManager>();
        

        Assert.IsNotNull(player, "[AIManager] player is null!");
        Assert.IsNotNull(context, "[AIManager] context is null!");
        Assert.IsNotNull(scanner, "[AIManager] scanner is null!");
        Assert.IsNotNull(moveToPickup, "[AIManager] moveToPickup is null!");
        Assert.IsNotNull(playerController, "[AIManager] playerController is null!");
        Assert.IsNotNull(moveToBestPosition, "[AIManager] moveToBestPosition is null!");
        Assert.IsNotNull(useHealth, "[AIManager] useHealth is null!");
        Assert.IsNotNull(reloadGun, "[AIManager] reloadGun is null!");
        Assert.IsNotNull(setBestAttackTarget, "[AIManager] setBestAttackTarget is null!");
        Assert.IsNotNull(debugSphereManager, "[AIManager] debugSphereManager is null!");

        //debugPrefabHolder = new GameObject();
        //debugPrefabHolder.name = "debugPrefabHolder";
    }

    void Start()
    {
        InvokeRepeating("Scan", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
        InvokeRepeating("TacticalMovement", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
    }

    private void Scan()
    {
        //Debug.Log("Time: " + Time.time);
        scanner.ScanForEnemies(this.context, enemyScanRange);
        scanner.ScanForPickups(this.context, pickupScanRange);
        scanner.ScanForPositions(this.context, samplingRange, samplingDensity);
        //if (debugMode)
        //{
        //    debugSphereManager.UpdateSpheres(context.sampledPositions);
        //}

        /* TacticalMovement and PlayerAction runs in parallel */
        StartCoroutine(TacticalMovement());
        StartCoroutine(PlayerAction());
    }

    IEnumerator TacticalMovement()
    {
        if (HasEnemies())
        {
            /* MoveToPickup */
            float pickupScore = moveToPickup.Run(context);

            if (pickupScore > 0)
            {
                if(context.nearestPickup)
                {
                    Vector3 desiredPosition = (context.nearestPickup.transform.position);
                    playerController.desiredPositionByAI = desiredPosition;
                }
                yield return null;
            }

            float reloadScore = reloadGun.Run(context);
            if (reloadScore > 0)
            {
                Debug.Log("Trying to reload!");
                player.gunController.Reload();
                yield return null;
            }

            /* Tactical Move */
            //moveToBestPosition.Run(context);
            Vector3 bestPosition = moveToBestPosition.GetBest(context);

            if (debugMode)
            {
                /* Update spheres placement and scores */
                debugSphereManager.UpdateSpheres(context.sampledPositions, moveToBestPosition.scores);
            }

            playerController.desiredPositionByAI = bestPosition;
            yield return null; // Just to be sure
        }
        else
        {
            Debug.Log("[Idling TacticalMovemente] No enemy scanned");
        }
    }


    /*  First Score Wins Selector
     *  This means that the first Qualifier that scores more than zero (0)
     *  executes its action. */
    IEnumerator PlayerAction()
    {
        if (HasEnemies())
        {
            /* Use Health */
            float hpScore = useHealth.Run(context);

            if (hpScore > 0)
            {
                Debug.Log("Using HP");
                player.UseHealthPack();
                // Action -> use HP

                yield return null;
            }
            //Debug.Log("NOT Using HP");
            //TODO: /* Throw Bomb */

            //yield return null;

            //TODO: /* Reload Gun */

            //yield return null;

            //TODO: /* Fire Gun - Set Target and Fire Gun */
            float bestTargetScore = setBestAttackTarget.Run(context);
            if (bestTargetScore > 0)
            {
                //Debug.Log("Acquiring Enemy " + context.nearestEnemy.name);
                               
                player.targetEntity = context.nearestEnemy;

                //Vector2 error2D = Random.insideUnitCircle * aiErrorFactor;
                //Vector3 error3D = new Vector3(error2D.x, player.targetEntity.transform.position.y, error2D.y);
                //desiredShootingPosition = player.targetEntity.transform.position + error3D;

                /* Fire Gun at target acquired */
                player.crosshairs.transform.position = context.nearestEnemy.transform.position;
                player.AimAndShoot(coolDownToShoot);
            }
            //Debug.Log("bestTargetScore: " + bestTargetScore);
            yield return null;

            //TODO: /* Default Action - Idle */

            //yield return null;

        } else
        {
            Debug.Log("[Idling PlayerActions] No enemy scanned");
            yield return null;
        }
    }
    
    public bool HasEnemies()
    {
        return (context.enemies.Count != 0);
    }

}
