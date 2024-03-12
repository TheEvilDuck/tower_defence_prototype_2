using System;
using UnityEngine;

public class PositionAnimation : Animation
{
    protected readonly Transform _targetTrasform;
    protected Vector2 _offset;
    protected Vector2 _startPosition;
    public PositionAnimation(AnimationValueUpdater updater, Transform targetTransform, Vector2 offset) : base(updater)
    {
        _targetTrasform = targetTransform;
        _offset = offset;
        _startPosition = _targetTrasform.position;
    }

    public void ChangeOffset(Vector2 offset)
    {
        _offset = offset;
        _startPosition = _targetTrasform.position;
    }

    protected override void OnStop()
    {
        _targetTrasform.position = _startPosition;
    }

    protected override void OnUpdate(float value)
    {
        _targetTrasform.position = _startPosition+_offset*value;
    }
}