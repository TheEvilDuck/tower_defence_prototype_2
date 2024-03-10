using System;

public abstract class Animation
{
    public event Action<Animation> end;
    protected readonly AnimationValueUpdater _updater;
    private bool _playing;

    public Animation(AnimationValueUpdater updater)
    {
        _updater = updater;
    }

    public void SetPause(bool pause)
    {
        _playing = pause;
    }

    public void Start()
    {
        _playing = true;
        _updater.Reset();
    }

    public void Stop()
    {
        _playing = false;
        OnStop();
        end?.Invoke(this);
    }

    public void Update()
    {
        if (!_playing)
            return;

        if (_updater.StepsLeft<=0)
            Stop();

        float value = _updater.GetNextValue();
        OnUpdate(value);

        _updater.StepsLeft--;

        if (_updater.StepsLeft==0)
            Stop();
    }

    protected abstract void OnUpdate(float value);
    protected abstract void OnStop();
}