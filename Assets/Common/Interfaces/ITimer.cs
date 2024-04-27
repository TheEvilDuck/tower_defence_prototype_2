using System;

namespace Common.Interfaces
{
    public interface ITimer: IPausable
    {
        public event Action<float> ticked;
    }

}