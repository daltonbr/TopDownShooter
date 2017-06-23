using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Context : MonoBehaviour {

    public static Context instance;
    public Player player1;
    public Scanner scanner;
    public float scanTimeIntervalInMs = 1;

    private float nextScanTime;

    public Context(Player entity)
    {
        this.player = entity;
        this.enemies = new List<LivingEntity>();
        this.sampledPositions = new List<Vector3>();
        this.powerups = new List<Pickup>();
    }

    void OnAwake()
    {
        //Assert.IsNotNull(player1, "[Context] player1 is not null!");

        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        instance.player = player1;
        scanner = this.gameObject.GetComponent<Scanner>();
        Assert.IsNotNull(scanner, "[Context] scanner is null!");
        //instance = new Context(player1);
        
    }

    private void FixedUpdate()
    {
        if (Time.time > nextScanTime)
        {
            Debug.Log("Time: " + Time.time);
            scanner.ScanForEnemies(Context.instance);
            nextScanTime = Time.time + scanTimeIntervalInMs;
        }
    }


    public Player player { get; private set; }

    public List<LivingEntity> enemies { get; private set; }

    public List<Pickup> powerups
    { get; private set; }

    public List<Vector3> sampledPositions { get; private set; }
}
