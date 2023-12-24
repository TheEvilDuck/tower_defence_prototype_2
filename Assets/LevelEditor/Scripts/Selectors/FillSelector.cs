using System;
using System.Collections;
using System.Collections.Generic;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Selectors
{
    public class FillSelector : IDisposable, ISelector
    {
        public event Action<Vector2Int> selectedCellsChanged;
        public event Action cellsSelected;
        public event Action<Vector2Int> selectionStarted;

        private readonly PlayerInput _playerInput;
        private readonly Grid _grid;
        private List<Vector2Int>_selectedCells;

        private readonly Vector2Int[] _directions = {Vector2Int.right,Vector2Int.up,Vector2Int.left,Vector2Int.down};

        public FillSelector(PlayerInput playerInput, Grid grid)
        {
            _playerInput = playerInput;
            _grid = grid;
        }

        public void Disable()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
        }

        public void Dispose()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
        }

        public void Enable()
        {
            _selectedCells = new List<Vector2Int>();
            _playerInput.mouseLeftClicked+=OnMouseLeftDown;
        }

        private void OnMouseLeftDown(Vector2 mousePosition)
        {
            _selectedCells.Clear();
            Vector2Int cellPosition = _grid.WorldPositionToGridPosition(mousePosition);

            if (!_grid.IsPositionValid(cellPosition)||_grid.IsCellAt(cellPosition))
                return;

            selectionStarted?.Invoke(cellPosition);

            _selectedCells.Add(cellPosition);

            SelectCellsAround(cellPosition);
            cellsSelected?.Invoke();

        }

        private void SelectCellsAround(Vector2Int center)
        {
            foreach (Vector2Int direction in _directions)
            {
                if (!_selectedCells.Contains(center+direction)&&_grid.IsPositionValid(center+direction)&&!_grid.IsCellAt(center+direction))
                {
                    _selectedCells.Add(center+direction);
                    selectedCellsChanged?.Invoke(center+direction);
                    SelectCellsAround(center+direction);
                }
            }
        }
    }
}
