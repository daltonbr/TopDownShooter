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
    public float samplingDensity = 1.5f;
    public float samplingRange = 12f;

    void Awake()
    {
        player = this.gameObject.GetComponent<Player>();
        this.context = new Context(player);
        this.scanner = this.gameObject.AddComponent<Scanner>();

        Assert.IsNotNull(player, "[AIManager] player is null!");
        Assert.IsNotNull(context, "[AIManager] context is null!");
        Assert.IsNotNull(scanner, "[AIManager] scanner is null!");
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
    }


}
