

public class AnimationValueRepeater : IAnimationValueUpdater
{
    private readonly IAnimationValueUpdater _wrapperUpdater;
    private readonly int _reps;
    private int _repLeft;

    public AnimationValueRepeater(IAnimationValueUpdater animationValueUpdater, int repeatCount)
    {
        _wrapperUpdater = animationValueUpdater;
        _reps = repeatCount;
        _repLeft = repeatCount;
    }

    public bool End => _repLeft == 0;

    public float GetNextValue()
    {
        float res = _wrapperUpdater.GetNextValue();

        if (_wrapperUpdater.End)
        {
            _wrapperUpdater.Reset();
            _repLeft--;
        }

        return res;
    }

    public void Reset()
    {
        _wrapperUpdater.Reset();
        _repLeft = _reps;
    }
}