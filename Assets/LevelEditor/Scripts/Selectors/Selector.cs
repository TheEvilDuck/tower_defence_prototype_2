using System;
using UnityEngine;

namespace LevelEditor.Selectors
{
    public interface ISelector
    {
        public event Action<Vector2Int, bool> selectedCellsChanged;
        public event Action cellsSelected;
        public event Action<Vector2Int> selectionStarted;

        public void Enable();
        public void Disable();
    }
}
