public interface IAnimationValueUpdater
{
    public bool End {get;}
    public void Reset();
    public float GetNextValue();
}