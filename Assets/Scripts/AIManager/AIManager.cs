using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AIManager : MonoBehaviour
{
    public Player player;
    public Context context;
    public Scanner scanner;
    
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
        player = this.gameObject.GetComponent<Player>();
        this.context = new Context(player);
        this.scanner = this.gameObject.AddComponent<Scanner>();

        Assert.IsNotNull(player, "[AIManager] player is null!");
        Assert.IsNotNull(context, "[AIManager] context is null!");
        Assert.IsNotNull(scanner, "[AIManager] scanner is null!");
        debugPrefabHolder = new GameObject();
        debugPrefabHolder.name = "debugPrefabHolder";
    }

    void Start()
    {
        InvokeRepeating("Scan", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
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
