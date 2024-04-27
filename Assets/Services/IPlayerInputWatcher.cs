using System;

namespace Services.PlayerInput
{
    public interface IPlayerInputWatcher
    {
        public event Action Down;
        public event Action Hold;
    }
}
