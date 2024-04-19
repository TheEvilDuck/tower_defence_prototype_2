using System;
using Common.Interfaces;

namespace BuffSystem
{
    public class TempBuff<TStats> : IBuff<TStats> where TStats : IStats
    {
        private readonly ITimer _timer;
        private readonly IBuffable<TStats> _buffable;
        private readonly IBuff<TStats> _buff;
        private float _time;

        public TempBuff(float time, ITimer timer, IBuffable<TStats> buffable, IBuff<TStats> buff)
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