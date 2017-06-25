using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToBestPosition
{
    public PositionProximityToSelf positionProximityToSelf;
    public ProximityToNearestEnemy proximityToNearestEnemy;

    public MoveToBestPosition()
    {
        positionProximityToSelf = new PositionProximityToSelf();
        proximityToNearestEnemy = new ProximityToNearestEnemy();
    }

    float score1, score2;

    public Vector3 GetBest(Context context)
    {
        //Debug.Log("Getbest");
        List<Vector3> positions = context.sampledPositions;
        int positionsCount = positions.Count;

        //List<float> scores = new List<float>();//context.sampledPositionsValues;
        float[] scores = new float[positionsCount];
        //scores.Clear();

        for (int i = 0; i < positionsCount; i++)
        {
            scores[i] = positionProximityToSelf.Score(context, positions[i]);
            scores[i] += proximityToNearestEnemy.Score(context, positions[i]);
            //Debug.Log("Score[" + i + "]: " + scores[i]);
        }

        //TODO: /* Show the scores in the debug prefab */

        //TODO: /* Determine the index of the greater score*/
        int greaterScoreIndex = 0;
        float greaterScore = -1f;
        for (int i = 0; i < positionsCount; i++)
        {
            if (scores[i] > greaterScore)
            {
                greaterScore = scores[i];
                greaterScoreIndex = i;
            }            
        }
        Debug.Log("Score[" + greaterScoreIndex + "]: " + greaterScore);
        /* Return it's respective sampledPosition */
        return positions[greaterScoreIndex];
    }

}

public sealed class PositionProximityToSelf : CustomScorer<Vector3>
{
    public new float factor = 0.1f;
    public new float score = 10f;

    public override float Score(Context context, Vector3 position)
    {
        float range = (position - context.player.transform.position).magnitude;
        return Mathf.Max(0f, (this.score - range));
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