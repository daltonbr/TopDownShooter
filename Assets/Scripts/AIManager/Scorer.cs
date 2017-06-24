public abstract class Scorer
{
    public float score;
    public float range;
    public float desiredRange;
    public float multiplier;
    public float factor;

    public abstract float Score(Context context);

}