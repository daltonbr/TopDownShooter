using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Context : MonoBehaviour {

    //public Scanner scanner;

    //public Context(Player entity)
    //{
    //    this.player = entity;
    //    this.enemies = new List<Enemy>();
    //    this.sampledPositions = new List<Vector3>();
    //    this.powerups = new List<Pickup>();
    //}

    void OnAwake()
    {
        //Assert.IsNotNull(player1, "[Context] player1 is not null!");

        //if (instance != null)
        //{
        //    Destroy(gameObject);
        //}
        //else
        //{
        //    instance = this;
        //    DontDestroyOnLoad(gameObject);
        //}

        
        //scanner = this.gameObject.GetComponent<Scanner>();
        //Assert.IsNotNull(scanner, "[Context] scanner is null!");
        //instance = new Context(player1);
        
    }

    void OnStart()
    {
       // Context.instance.player = player1;
        //Debug.Log("Player in context: " + this.player.name);
    }

    public void SetPlayer (Player player)
    {
        this.player = player;
    }

    public Player player { get; private set; }

    public List<Enemy> enemies { get; private set; }

    public List<Pickup> powerups { get; private set; }

    public List<Vector3> sampledPositions { get; private set; }
}
