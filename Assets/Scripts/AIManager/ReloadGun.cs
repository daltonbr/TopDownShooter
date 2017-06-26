using UnityEngine;

/* Sum of Children - a sum of all Scorers, no threshold */
public class ReloadGun
{
    public Scorer isGunLoaded;

    public ReloadGun()
    {
        isGunLoaded = new IsGunLoaded();
    }

    public float Run(Context context)
    {
        return isGunLoaded.Score(context);
    }
}

/* TODO: refine this method to consider magazines too
 * Ideally put another Scorer */
public sealed class IsGunLoaded : Scorer
{
    new public float score = 100f;

    public override float Score(Context context)
    {
        if (context.player.HasAnyBulletInMagazine())
        {
            return 0f;
        }
        Debug.Log("Need to Reload!");
        return this.score;
    }
}