using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Scanner : MonoBehaviour {

    public void ScanForEnemies(Context context, float enemyScanRange)
    {
        Player player = context.player;
        List<Enemy> enemies = context.enemies;
        enemies.Clear();
 
        // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
        var colliders = Physics.OverlapSphere(player.transform.position, enemyScanRange, LayerMask.GetMask("Enemy"));
        
        for (int i = 0; i < colliders.Length; i++)
        {
            Enemy enemy = colliders[i].GetComponent<Enemy>();
            context.enemies.Add(enemy);
        }
        Debug.Log("Enemies.Lenght: " + context.enemies.Count);
    }

    public void ScanForPickups(Context context, float powerupScanRange)
    {
        Player player = context.player;
        List<Pickup> powerups = context.pickups;
        powerups.Clear();

        // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
        var colliders = Physics.OverlapSphere(player.transform.position, powerupScanRange, LayerMask.GetMask("Pickup"));

        for (int i = 0; i < colliders.Length; i++)
        {
            Pickup pickup = colliders[i].GetComponent<Pickup>();
            context.pickups.Add(pickup);
        }
        Debug.Log("pickups.Lenght: " + context.pickups.Count);
    }

    public void ScanForPositions(Context context)
    {
        Debug.Log("ScanForPositions not implemented yet");
    }
}

