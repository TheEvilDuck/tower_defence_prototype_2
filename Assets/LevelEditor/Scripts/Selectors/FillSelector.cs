using System;
using System.Collections.Generic;
using Levels.Logic;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Selectors
{
    public class FillSelector : IDisposable, ISelector
    {
        public event Action<Vector2Int, bool> selectedCellsChanged;
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

            if (!_grid.IsPositionValid(cellPosition))
                return;

            TileType referencedType = TileType.Empty;

            if (_grid.TryGetCellDataAt(cellPosition, out CellData cellData))
            {
                referencedType = cellData.Type;
            }

            selectionStarted?.Invoke(cellPosition);

            _selectedCells.Add(cellPosition);

            SelectCellsAround(cellPosition,referencedType);
            cellsSelected?.Invoke();

        }

        private void SelectCellsAround(Vector2Int center, TileType referenceCellType)
        {
            foreach (Vector2Int direction in _directions)
            {
                if (!_grid.IsPositionValid(center+direction))
                    continue;

                if (_grid.IsCellAt(center+direction)&&referenceCellType == TileType.Empty)
                    continue;

                _grid.TryGetCellDataAt(center+direction, out CellData cellData); 

                if (referenceCellType != cellData.Type)
                    continue;

                if (!_selectedCells.Contains(center+direction))
                {
                    _selectedCells.Add(center+direction);
                    selectedCellsChanged?.Invoke(center+direction, true);
                    SelectCellsAround(center+direction,referenceCellType);
                }
            }
        }
    }
}
