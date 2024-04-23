public class WiggleAnimationValueUpdater : SmoothAnimationValueUpdater
{
    public WiggleAnimationValueUpdater(float baseValue, float targetValue, int steps) : base(baseValue, targetValue, steps)
    {
    }

    public override float GetNextValue()
    {
        float smoothValue = base.GetNextValue();

        StepsLeft--;

        return smoothValue*UnityEngine.Random.Range(-1f,1f);
    }
}