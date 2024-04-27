using System;
using UnityEngine;

namespace Levels.Logic
{
    public interface IGridCellRemovedNotifier
    {
        public event Action<Vector2Int> cellRemoved;
    }
}