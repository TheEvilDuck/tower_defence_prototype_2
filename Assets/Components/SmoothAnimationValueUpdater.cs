using UnityEngine;

public class SmoothAnimationValueUpdater: AnimationValueUpdater
{
    private float _blending = 1f;

    public SmoothAnimationValueUpdater(float baseValue, float targetValue, int steps) : base(baseValue, targetValue, steps)
    {
    }

    public override float GetNextValue()
    {
        float result = Mathf.Lerp(_baseValue,_targetValue,_blending);
        _blending-=1f/_steps;

        StepsLeft--;

        return result;
    }

    public override void Reset()
    {
        base.Reset();

        _blending = 1f;
    }
}