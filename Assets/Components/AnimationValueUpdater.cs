public abstract class AnimationValueUpdater
{
    protected readonly float _baseValue;
    protected readonly float _targetValue;
    protected readonly int _steps;

    public int StepsLeft {get; set;}

    public AnimationValueUpdater(float baseValue, float targetValue, int steps)
    {
        _baseValue = baseValue;
        _targetValue = targetValue;
        _steps = steps;

        Reset();
    }

    public virtual void Reset()
    {
        StepsLeft = _steps;
    }

    public abstract float GetNextValue();
}