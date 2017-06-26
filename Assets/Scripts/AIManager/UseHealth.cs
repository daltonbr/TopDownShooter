using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UseHealth
{
    /*
     * -= All or Nothing =-
     * all Scorers need to score above the 'threshold' before we consider the outcome,
     * otherwise we will not consider anything
     */

    public float threshold = 100f;
    public Scorer healthBelowThreshold;
    public Scorer hasHealthPacks;

    /* Constructor */
    public UseHealth()
    {
        healthBelowThreshold = new HealthBelowThreshold();
        hasHealthPacks = new HasHealthPacks();
    }

    float scoreHealth, scoreHPs;
    public float Run(Context context)
    {
        /* check if both of these scores are above the threshold */
        scoreHealth = healthBelowThreshold.Score(context);
        scoreHPs = hasHealthPacks.Score(context);

        /* all or nothing */
        if (scoreHealth > threshold && scoreHPs > threshold)
        {
            var totalScore = scoreHealth + scoreHPs;
            return totalScore;
        }
        return 0f;
    }

}

/* Return the score when health is bellow the threshold */
public sealed class HealthBelowThreshold : Scorer
{
    // health threshold must be somenting between 0f and 1f
    public float threshold = .35f;
    new public float score = 200f;

    public override float Score(Context context)
    {
        if (context.player.GetCurrentHealthPercent() < threshold)
        {
            return this.score;
        }
        return 0f;
    }
}

/* Return this score when a player has any HP's */
public sealed class HasHealthPacks : Scorer
{
    public new float score = 200f;

    public override float Score(Context context)
    {
        if (context.player.currentHealthPacks >= 0)
        {
            return this.score;
        }
        return 0f;
    }
}