using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AIManager : MonoBehaviour
{
    public Player player1;
    public Context context;
    public Scanner scanner;

    [Header("Scanner")]
    public float scanTimeIntervalInSecs = 1f;
    [Range(0f, 30f)]
    public float enemyScanRange = 10f;
    [Range(0f, 30f)]
    public float powerupScanRange = 10f;

    [Header("Movement")]
    public float samplingDensity = 1.5f;
    public float samplingRange = 12f;

    void OnAwake()
    {
        
    }

    void OnStart()
    {
        this.context = player1.GetContext();
        Assert.IsNotNull(context, "[AIManager] player1 is null!");
        Assert.IsNotNull(context, "[AIManager] context is null!");
        Assert.IsNotNull(scanner, "[AIManager] scanner is null!");

        InvokeRepeating("Scan", scanTimeIntervalInSecs, scanTimeIntervalInSecs);
    }

    private void Scan()
    {
        Debug.Log("Time: " + Time.time);
        scanner.ScanForEnemies(this.context, enemyScanRange);
        scanner.ScanForPowerUps(this.context, powerupScanRange);
    }


}
