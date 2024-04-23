using System;

public abstract class Animation
{
    public event Action<Animation> end;
    protected readonly IAnimationValueUpdater _updater;
    private bool _playing;

    public Animation(IAnimationValueUpdater updater)
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

        if (_updater.End)
            Stop();

        float value = _updater.GetNextValue();
        OnUpdate(value);

        if (_updater.End)
            Stop();
    }

    protected abstract void OnUpdate(float value);
    protected abstract void OnStop();
}