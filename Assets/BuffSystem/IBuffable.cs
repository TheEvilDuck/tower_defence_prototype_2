namespace BuffSystem
{
    public interface IBuffable<TStats> where TStats: IStats
    {
        public void AddBuff(IBuff<TStats> buff);
        public void RemoveBuff(IBuff<TStats> buff);
    }
}