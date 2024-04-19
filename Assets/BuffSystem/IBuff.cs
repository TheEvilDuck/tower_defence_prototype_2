namespace BuffSystem
{
    public interface IBuff<TStats> where TStats: IStats
    {
        public TStats Apply(TStats baseStats);
    }
}