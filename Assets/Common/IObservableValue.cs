using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IObservableValue<T>
    {
        public event Action<T> changed;
    }
}
