using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBestPosition
{
    public PositionProximityToSelf positionProximityToSelf;
    public ProximityToNearestEnemy proximityToNearestEnemy;
    public OverRangeToClosestEnemy overRangeToClosestEnemy;
    public OverRangeToAnyEnemy overRangeToAnyEnemy;
    public LineOfSightToClosestEnemy lineOfSightToClosestEnemy;
    public LineOfSightToAnyEnemy lineOfSightToAnyEnemy;
    public ProximityToClosestPickup proximityToClosestPickup;
    public ProximityToPlayerSpawner proximityToPlayerSpawner;
    public float[] scores;
    int greaterScoreIndex;
    float greaterScore;

    public MoveToBestPosition()
    {
        positionProximityToSelf = new PositionProximityToSelf();
        proximityToNearestEnemy = new ProximityToNearestEnemy();
        overRangeToClosestEnemy = new OverRangeToClosestEnemy();
        overRangeToAnyEnemy = new OverRangeToAnyEnemy();
        lineOfSightToClosestEnemy = new LineOfSightToClosestEnemy();
        lineOfSightToAnyEnemy = new LineOfSightToAnyEnemy();
        proximityToClosestPickup = new ProximityToClosestPickup();
        proximityToPlayerSpawner = new ProximityToPlayerSpawner();
    }

    public Vector3 GetBest(Context context)
    {
        List<Vector3> positions = context.sampledPositions;
        int positionsCount = positions.Count;

        scores = new float[positionsCount];
        greaterScoreIndex = -1;
        greaterScore = -1f;

        for (int i = 0; i < positionsCount; i++)
        {
            scores[i] += positionProximityToSelf.Score(context, positions[i]);
            scores[i] += proximityToNearestEnemy.Score(context, positions[i]);
            scores[i] += overRangeToClosestEnemy.Score(context, positions[i]);
            scores[i] += overRangeToAnyEnemy.Score(context, positions[i]);
            scores[i] += lineOfSightToClosestEnemy.Score(context, positions[i]);
            scores[i] += lineOfSightToAnyEnemy.Score(context, positions[i]);
            scores[i] += proximityToClosestPickup.Score(context, positions[i]);
            scores[i] += proximityToPlayerSpawner.Score(context, positions[i]);

            if (greaterScore < scores[i])
            {
                greaterScoreIndex = i;
                greaterScore = scores[i];
            }
        }
        
        return positions[greaterScoreIndex];

        //TODO: /* Show the scores in the debug debugSpherePrefab */

        //TODO: /* Determine the index of the greater score*/
        //int greaterScoreIndex = 0;
        //float greaterScore = -1f;
        //for (int i = 0; i < positionsCount; i++)
        //{
        //    if (scores[i] > greaterScore)
        //    {
        //        greaterScore = scores[i];
        //        greaterScoreIndex = i;
        //    }
        //}

        /* Return it's respective sampledPosition */
        //Debug.Log("Score[" + greaterScoreIndex + "]: " + greaterScore);
        // return positions[greaterScoreIndex];
    }

    //public Vector3 GetBest(Context context)
    //{
    //    return context.sampledPositions[greaterScoreIndex];
    //}

    public void UpdateScoresOnDebugSpheres(DebugSphere[] spheres)
    {
        if (scores != null)
        {
            Debug.LogWarning("scores is null! Probably not calculated yet!");
            return;
        }
        for (int i = 0; i < spheres.Length; i++)
        {
            spheres[i].SetScore(scores[i]);
        }
    }

}

public sealed class PositionProximityToSelf : CustomScorer<Vector3>
{
    public new float factor = 0.1f;
    public new float score = 10f;

    public override float Score(Context context, Vector3 position)
    {
        float range = (position - context.player.transform.position).magnitude;
        return factor * Mathf.Max(0f, (this.score - range));
    }
}

public sealed class ProximityToNearestEnemy : CustomScorer<Vector3>
{
    public new float desiredRange = 8f;
    public new float score = 50f;

    public override float Score(Context context, Vector3 position)
    {
        List<Enemy> enemies = context.enemies;
        float count = enemies.Count;
        if (count == 0) { return 0f; }

        Vector3 nearest = Vector3.zero;
        float shortest = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];

            float distance = (position - enemy.transform.position).sqrMagnitude;
            if (distance < shortest)
            {
                shortest = distance;
                nearest = enemy.transform.position;
            }
        }

        if (nearest.sqrMagnitude == 0f)
        {
            return 0f;
        }

        float range = (position - nearest).magnitude;
        return Mathf.Max(0f, (this.score - Mathf.Abs(this.desiredRange - range)));
    }
}

public sealed class OverRangeToClosestEnemy : CustomScorer<Vector3>
{
    new public float desiredRange = 5f;
    new public float score = 100f;

    public override float Score(Context context, Vector3 position)
    {
        Player player = context.player;

        List<Enemy> enemies = context.enemies;
        int count = enemies.Count;
        if (count == 0)
        {
            return 0f;
        }

        Vector3 nearest = Vector3.zero;
        float shortest = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];

            float distance = (player.transform.position - enemy.transform.position).sqrMagnitude;
            if (distance < shortest)
            {
                shortest = distance;
                nearest = enemy.transform.position;
            }
        }

        float range = (position - nearest).magnitude;
        if (range > desiredRange)
        {
            return this.score;
        }
        else
        {
            return 0;
        }
    }
}

public sealed class OverRangeToAnyEnemy : CustomScorer<Vector3>
{
    new public float desiredRange = 5f;
    new public float score = 50f;

    public override float Score(Context context, Vector3 position)
    {
        Player player = context.player;

        List<Enemy> enemies = context.enemies;
        int count = enemies.Count;
        if (count == 0)
        {
            return 0f;
        }

        float sqrDesiredRange = desiredRange * desiredRange;
        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];

            Vector3 dirPlayerToEnemy = (enemy.transform.position - player.transform.position);
            Vector3 dirPositionToEnemy = (enemy.transform.position - position);

            dirPlayerToEnemy = new Vector3(dirPlayerToEnemy.x, 0f, dirPlayerToEnemy.z);
            dirPositionToEnemy = new Vector3(dirPositionToEnemy.x, 0f, dirPositionToEnemy.z);

            //all positions behind the enemy or closer than the desired range are not of interest
            if (Vector3.Dot(dirPlayerToEnemy, dirPositionToEnemy) < 0f || dirPositionToEnemy.sqrMagnitude < sqrDesiredRange)
            {
                return 0f;
            }
        }

        return this.score;
    }


}

public sealed class LineOfSightToClosestEnemy : CustomScorer<Vector3>
{
    new public float score = 50f;

    public override float Score(Context context, Vector3 position)
    {
        Player player = context.player;

        List<Enemy> enemies = context.enemies;
        int count = enemies.Count;
        if (count == 0)
        {
            return 0f;
        }

        var nearest = Vector3.zero;
        var shortest = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];

            float distance = (player.transform.position - enemy.transform.position).sqrMagnitude;
            if (distance < shortest)
            {
                shortest = distance;
                nearest = enemy.transform.position;
            }
        }

        Vector3 dir = (nearest - position);
        float range = dir.magnitude;
        Ray ray = new Ray(position + Vector3.up, dir);

        if (!Physics.Raycast(ray, range, LayerMask.GetMask("Obstacle")))
        {
            return this.score;
        }

        return 0f;
    }
}

public sealed class LineOfSightToAnyEnemy : CustomScorer<Vector3>
{
    new public float score = 50f;

    public override float Score(Context context, Vector3 position)
    {
        List<Enemy> enemies = context.enemies;
        int count = enemies.Count;
        if (count == 0)
        {
            return 0f;
        }

        for (int i = 0; i < count; i++)
        {
            Enemy enemy = enemies[i];
            Vector3 dir = enemy.transform.position - position;
            float range = dir.magnitude;
            Ray ray = new Ray(position + Vector3.up, dir);

            if (!Physics.Raycast(ray, range, LayerMask.GetMask("Obstacle")))
            {
                return this.score;
            }
        }

        return 0;
    }
}

public sealed class ProximityToClosestPickup : CustomScorer<Vector3>
{
    new public float multiplier = 2f;
    new public float score = 20f;

    public override float Score(Context context, Vector3 position)
    {
        List<Pickup> pickups = context.pickups;
        int count = pickups.Count;
        if (count == 0)
        {
            return 0f;
        }

        Vector3 closest = Vector3.zero;
        float shortest = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            var pickup = pickups[i];

            float distance = (position - pickup.transform.position).sqrMagnitude;
            if (distance < shortest)
            {
                shortest = distance;
                closest = pickup.transform.position;
            }
        }

        var range = (position - closest).magnitude;
        return Mathf.Max(0f, (this.score - range) * multiplier);
    }
}

public sealed class ProximityToPlayerSpawner : CustomScorer<Vector3>
{
    new public float multiplier = 1f;
    new public float score = 100f;

    public override float Score(Context context, Vector3 position)
    {
        float range = (position - context.player.spawnPoint).magnitude;
        return Mathf.Max(0f, (this.score - range) * multiplier);
    }
}