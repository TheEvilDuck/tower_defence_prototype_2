using System;

namespace BuffSystem
{
    public interface IBuffable<TStats, BuffID> 
        where TStats: IStats
        where BuffID: Enum
    {
        public void AddBuff(IBuff<TStats, BuffID> buff);
        public void RemoveBuff(IBuff<TStats, BuffID> buff);
    }
}