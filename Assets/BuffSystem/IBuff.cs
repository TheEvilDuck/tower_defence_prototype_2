using System;

namespace BuffSystem
{
    public interface IBuff<TStats, BuffID> 
        where TStats: IStats 
        where BuffID: Enum
    {
        public TStats Apply(TStats baseStats);
        public BuffID Id {get;}
    }
}