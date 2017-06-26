using UnityEngine;

public class SetBestAttackTarget
{
    public Scorer enemyProximityToSelf;
    public Scorer isCurrentTargetScorer;

    /* Constructor */
    public SetBestAttackTarget()
    {
        enemyProximityToSelf = new EnemyProximityToSelf();
        isCurrentTargetScorer = new IsCurrentTargetScorer();
    }

    public float Run(Context context)
    {
        float score = enemyProximityToSelf.Score(context);
        score += isCurrentTargetScorer.Score(context);

        return score;
    }
}

public sealed class EnemyProximityToSelf : Scorer
{
    new public float multiplier = 1f;
    new public float score = 50f;

    public override float Score(Context context)
    {
        if (context.nearestEnemy != null)
        {
            Debug.Log("Nearest Enemy " + context.nearestEnemy.name);
            float distance = (context.nearestEnemy.transform.position - context.player.transform.position).magnitude;
            return Mathf.Max(0f, (this.score - distance) * this.multiplier);
        }
        return 0f;
    }
}

/* Doesn't need to implement,
 * since our enemy when dead is automatically destroyed */
//public sealed class IsAliveScorer : CustomScorer<Enemy>
//{
//    public override float Score(Context context, Enemy enemy)

//        if (enemy.currentHealth > 0)
//        {
//            return this.score;
//        }
//        return 0f;
//    }
//}

public sealed class IsCurrentTargetScorer : Scorer
{
    new public float score = 1.5f;

    public override float Score(Context context)
    {
        if (context.nearestEnemy != null)
        {
            //TODO: or should we user Equals() ?
            if (context.nearestEnemy == context.player.targetEntity)
            {
                return this.score;
            }
        }
        return 0f;
    }
}