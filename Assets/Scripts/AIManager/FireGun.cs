using UnityEngine;

/* Sum of Children - a sum of all Scorers, no threshold */
public class FireGun {

    public Scorer isGunLoaded;

    public FireGun()
    {
        isGunLoaded = new IsGunLoaded();
        /* override the score 
         * since we are reusing this class */
        isGunLoaded.score = 10f;
    }

    public float Run(Context context)
    {
        return isGunLoaded.Score(context);
    }
}