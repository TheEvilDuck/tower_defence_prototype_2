using System;

namespace Levels.Logic
{
    public class Cell: IDisposable
    {
        public event Action cellChanged;

        public bool HasRoad {get; private set;}

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

        public void Dispose()
        {
            cellChanged = null;
        }
    }
}
