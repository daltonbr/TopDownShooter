using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveToPickup
{
    /*
     * -= All or Nothing =-
     * all Scorers need to score above the 'threshold' before we consider the outcome,
     * otherwise we will not consider anything
     */

    public float threshold = 100f;
    public Scorer hasEnemiesInRange;
    public Scorer hasPickupsInRange;

    public MoveToPickup()
    {
        hasEnemiesInRange = new HasEnemiesInRange();
        hasPickupsInRange = new HasPickupsInRange();
    }

    float scoreEnemies, scorePickups;
    public float Run(Context context)
    {
        // check if both of these scores are above the threshold
        scoreEnemies = hasEnemiesInRange.Score(context);
        scorePickups = hasPickupsInRange.Score(context);

        /* all or nothing */
        
        if (scoreEnemies > threshold && scorePickups > threshold)
        {
            var totalScore = scoreEnemies + scorePickups;
            //Debug.Log("MoveToPickup Score: " + totalScore);
            return totalScore;
        }
        //Debug.Log("MoveToPickup Score 0");
        return 0f;
    }

}


public sealed class HasEnemiesInRange : Scorer
{
    public new float range = 15f;
    public new float score = 200f;

    public override float Score(Context context)
    {
        List<Enemy> enemies = context.enemies;
        int count = enemies.Count;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];
            float sqrDist = (enemy.transform.position - context.player.transform.position).sqrMagnitude;

            if (sqrDist <= range * range)
            {
                return this.score;
            }
        }
        return 0f;

    }
}


public sealed class HasPickupsInRange : Scorer
{
    public new float range = 8f;
    public new float score = 200f;

    public override float Score(Context context)
    {
        List<Pickup> pickups = context.pickups;
        int count = pickups.Count;

        for (int i = 0; i < count; i++)
        {
            Pickup pickup = pickups[i];
            float sqrDist = (pickup.transform.position - context.player.transform.position).sqrMagnitude;

            if (sqrDist <= range * range)
            {
                return this.score;
            }
        }
        return 0f;
    }
}