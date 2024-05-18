using System;
using Common.Interfaces;

namespace BuffSystem
{
    public class TempBuff<TStats, BuffID> : IBuff<TStats, BuffID> 
        where TStats : IStats
        where BuffID: Enum
    {
        private readonly ITimer _timer;
        private readonly IBuffable<TStats, BuffID> _buffable;
        private readonly IBuff<TStats, BuffID> _buff;
        private float _time;

        public BuffID Id {get; private set;}

        public TempBuff(float time, ITimer timer, IBuffable<TStats, BuffID> buffable, IBuff<TStats, BuffID> buff)
        {
            _timer = timer;
            _buffable = buffable;
            _buff = buff;
            _time = time;

            Action<float> onTick = null;

            onTick = (deltaTime) =>
            {
                _time -= deltaTime;

                if (_time <= 0)
                {
                    _buffable.RemoveBuff(this);
                    _timer.ticked -= onTick;
                }
            };

            _timer.ticked += onTick;
        }

        

        public TStats Apply(TStats baseStats)
        {
            return _buff.Apply(baseStats);
        }
    }
}