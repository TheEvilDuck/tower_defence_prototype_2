using System;
using Towers;

namespace Levels.Logic
{
    public class Cell: IDisposable
    {
        public event Action cellChanged;

        private Placable _placable;

        public bool HasRoad {get; private set;}
        public Placable Placable => _placable;

        public void BuildRoad()
        {
            if (HasRoad)
                return;

            HasRoad = true;
            cellChanged?.Invoke();
        }

        public void RemoveRoad()
        {
            if (!HasRoad)
                return;

            HasRoad = false;
            cellChanged?.Invoke();
        }

        public bool TryPlace(Placable placable)
        {
            if (_placable!=null)
                return false;

            _placable = placable;
            return true;
        }

        public bool TryDestroyPlacable()
        {
            if (_placable==null)
                return false;

            if (!_placable.CanBeDestroyed)
                return false;

            _placable.DestroyPlacable();
            _placable = null;

            return true;
        }

        public void Dispose()
        {
            TryDestroyPlacable();

            cellChanged = null;
        }
    }
}
