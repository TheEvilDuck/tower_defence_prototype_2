using System;
using System.Collections.Generic;
using Common.Interfaces;

namespace Common
{
    public class PausableManager: IPausable
    {
        private List<IPausable> _pausables;

        public bool IsPause {get; private set;}

        public PausableManager()
        {
            _pausables = new List<IPausable>();
        }

        public void Add(IPausable pausable)
        {
            if (pausable == this)
                throw new ArgumentException("Pauseable maanager must not contain itself");

            if (_pausables.Contains(pausable))
                throw new ArgumentException("Pauseable maanager must not contain dublicates");

            _pausables.Add(pausable);
        }

        public void Pause()
        {
            foreach (IPausable pausable in _pausables)
                pausable.Pause();

            IsPause = true;
        }

        public void UnPause()
        {
            foreach (IPausable pausable in _pausables)
                pausable.UnPause();

            IsPause = false;
        }
    }
}
