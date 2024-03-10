using UnityEngine;

public class RandomizedPositionAnimation : PositionAnimation
{
    public RandomizedPositionAnimation(AnimationValueUpdater updater, Transform targetTransform, Vector2 offset) : base(updater, targetTransform, offset)
    {
    }

    protected override void OnUpdate(float value)
    {
        base.OnUpdate(value);

        _targetTrasform.position += new Vector3(0, _offset.x*_updater.GetNextValue(),0);
    }
}