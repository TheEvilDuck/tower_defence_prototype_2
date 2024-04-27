using System;

namespace Common.Interfaces
{
    public interface IObservableValue<T>
    {
        public event Action<T> changed;
        public T Value {get;}
    }
}
