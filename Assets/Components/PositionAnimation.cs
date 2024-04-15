using System;
using UnityEngine;

public class PositionAnimation : Animation
{
    protected readonly Transform _targetTrasform;
    protected Vector2 _offset;
    protected Vector2 _startPosition;
    public PositionAnimation(AnimationValueUpdater updater, Transform targetTransform, Vector2 offset, Vector2 startPosition) : base(updater)
    {
        _targetTrasform = targetTransform;
        _offset = offset;
        _startPosition = startPosition;
    }

    public void ChangeOffset(Vector2 offset)
    {
        _offset = offset;
    }

    protected override void OnStop()
    {
        _targetTrasform.localPosition = _startPosition;
    }

    protected override void OnUpdate(float value)
    {
        _targetTrasform.localPosition = _startPosition+_offset*value;
    }
}