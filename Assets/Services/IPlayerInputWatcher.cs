using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlayerInput
{
    public interface IPlayerInputWatcher
    {
        public event Action Down;
        public event Action Hold;
    }
}
