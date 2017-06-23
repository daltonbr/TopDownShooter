using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Scanner : MonoBehaviour {



    public void ScanForEnemies(Context context, float enemyScanRange)
    {
        //Context c = Context.instance;

        Player player = context.player;
        context.enemies.Clear();

        // Use OverlapSphere for getting all relevant colliders within scan range, filtered by the scanning layer
        var colliders = Physics.OverlapSphere(player.transform.position, enemyScanRange/*player.scanRange*/, LayerMask.NameToLayer("Enemy"));

        for (int i = 0; i < colliders.Length; i++)
        {
            //var col = colliders[i];
            //if (col.isTrigger)
            //{
            //    Debug.Log("Collider is trigger " + col.gameObject.name);
            //    continue;
            //}


            //var enemy = EntityManager.instance.GetLivingEntityByGameObject(col.gameObject);
            //if (enemy == null)
            //{
            //    continue;
            //}

            //GameObject enemy = colliders[i].gameObject;
            Enemy enemy = colliders[i].GetComponent<Enemy>();
            Debug.Log("Collider is trigger " + enemy.gameObject.name);
            context.enemies.Add(enemy);
        }
    }

    public void ScanForPowerUps(Context context, float powerupScanRange)
    {
        Debug.Log("ScanForPowerUps not implemented yet");
    }

    public void ScanForPositions(Context context)
    {
        Debug.Log("ScanForPositions not implemented yet");
    }
}

