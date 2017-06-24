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

    public MoveToPickup moveToPickup;
    
    [Header("Scanner")]
    public float scanTimeIntervalInSecs = 1f;
    [Range(0f, 30f)]
    public float enemyScanRange = 10f;
    [Range(0f, 30f)]
    public float pickupScanRange = 10f;

    [Header("Movement")]
    [Range(0.5f, 3f)]
    public float samplingDensity = 1.5f;
    [Range(3f, 30f)]
    public float samplingRange = 12f;

    [Header("Debug")]
    public bool debugMode = true;
    public GameObject debugPrefab;
    //[Range(0f, 1f)]
    //public float transparency = 0.25f;

    GameObject debugPrefabHolder;
    

    void Awake()
    {
        this.player = this.gameObject.GetComponent<Player>();
        this.context = new Context(player);
        this.scanner = this.gameObject.AddComponent<Scanner>();
        this.moveToPickup = new MoveToPickup(); //this.gameObject.AddComponent<MoveToPickup>();
        this.playerController = player.GetComponent<PlayerController>();

        Assert.IsNotNull(player, "[AIManager] player is null!");
        Assert.IsNotNull(context, "[AIManager] context is null!");
        Assert.IsNotNull(scanner, "[AIManager] scanner is null!");
        Assert.IsNotNull(moveToPickup, "[AIManager] moveToPickup is null!");
        Assert.IsNotNull(playerController, "[AIManager] playerController is null!");

        debugPrefabHolder = new GameObject();
        debugPrefabHolder.name = "debugPrefabHolder";
    }

    void Start()
    {
        InvokeRepeating("Scan", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
        //InvokeRepeating("TacticalMovement", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
    }

    private void Scan()
    {
        //Debug.Log("Time: " + Time.time);
        scanner.ScanForEnemies(this.context, enemyScanRange);
        scanner.ScanForPickups(this.context, pickupScanRange);
        scanner.ScanForPositions(this.context, samplingRange, samplingDensity);
        if (debugMode)
        {
            DebugPositions(this.context.sampledPositions);
        }

        TacticalMovement();
    }

    private void TacticalMovement()
    {
        if (HasEnemies())
        {
            //Debug.Log("Has Enemies");
            /* MoveToPickup */
            float score = moveToPickup.Run(context);
            //Debug.Log("MoveToPickup score: " + score);
            if (score > 0)
            {
                Vector3 desiredPosition = (context.GetNearestPickup().transform.position);
                if (desiredPosition != null)
                {
                    playerController.desiredPositionByAI = desiredPosition;
                }
                return;
            }
            /* Tactical Move */
        }
        else
        {
            Debug.Log("[Idling] No enemy scanned");
        }
    }

  
    public bool HasEnemies()
    {
        return (context.enemies.Count != 0);
    }

    public void DebugPositions(List<Vector3> positions)
    {
        //TODO: [Optimization] make a pool with these prefabs   
        foreach (var v in positions)
        {
            Destroy(Instantiate(debugPrefab, v, Quaternion.identity, debugPrefabHolder.gameObject.transform), 1f) ;
        }
    }

}
