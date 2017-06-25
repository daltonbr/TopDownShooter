using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Context
{
    public Context(Player entity)
    {
        this.player = entity;
        this.enemies = new List<Enemy>();
        this.sampledPositions = new List<Vector3>();
        this.pickups = new List<Pickup>();
        //this.sampledPositionsValues = new List<float>();
    }

    public void SetPlayer (Player player)
    {
        this.player = player;
    }

    public Player player { get; private set; }

    public List<Enemy> enemies { get; private set; }

    public List<Pickup> pickups { get; private set; }

    public List<Vector3> sampledPositions { get; private set; }

    //public List<float> sampledPositionsValues { get; private set; }
    
    //public int getSampledPositionsValueByIndex(int index)
    //{
    //    return sampledPositionsValues[index];
    //}

    //public void setSampledPositionsValueByIndex(int index, float value)
    //{
    //    if (index < 0 || index > this.sampledPositions.Count -1)
    //    {
    //        Debug.LogError("[Context] Trying to set a value out-of-bounds!");
    //        return;
    //    }
    //    sampledPositionsValues[index] = value;
    //}

    public Pickup GetNearestPickup()
    {
        Pickup nearest = null;
        float minorDistSqr = Mathf.Infinity;

        foreach (var pickup in this.pickups)
        {
            float sqrDist = (player.transform.position - pickup.transform.position).sqrMagnitude;
            if (sqrDist < minorDistSqr)
            {
                minorDistSqr = sqrDist;
                nearest = pickup;
            }
        }
        return nearest;
    }
}
