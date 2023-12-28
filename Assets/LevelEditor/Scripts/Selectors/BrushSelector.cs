using System;
using System.Collections.Generic;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Selectors
{
    public class BrushSelector: IDisposable, ISelector 
    {
        public event Action<Vector2Int, bool> selectedCellsChanged;
        public event Action cellsSelected;
        public event Action<Vector2Int> selectionStarted;
        private readonly PlayerInput _playerInput;
        private readonly Grid _grid;
        private List<Vector2Int>_selectedCells;
        private bool _inProgress;
        private Vector2Int _lastCellPosition;

        public BrushSelector(PlayerInput playerInput, Grid grid)
        {
            _playerInput = playerInput;
            _grid = grid;
        }

        private void OnMouseMoved(Vector2 mousePosition)
        {
            if (!_inProgress)
                return;

            Vector2Int cellPosition = _grid.WorldPositionToGridPosition(mousePosition);

            if (cellPosition!=_lastCellPosition)
            {
                _lastCellPosition = cellPosition;

                if (!_selectedCells.Contains(_lastCellPosition))
                {
                    _selectedCells.Add(_lastCellPosition);
                    selectedCellsChanged?.Invoke(_lastCellPosition, true);
                }
            }
        }
        private void OnMouseLeftDown(Vector2 mousePosition)
        {
            _inProgress = true;
            _selectedCells.Clear();

            Vector2Int cellPosition = _grid.WorldPositionToGridPosition(mousePosition);
            _lastCellPosition =cellPosition;

            _selectedCells.Add(_lastCellPosition);

            selectionStarted?.Invoke(cellPosition);
        }
        private void OnMouseLeftUp(Vector2 mousePosition)
        {
            _inProgress = false;
            
            if (_selectedCells.Count>0)
                cellsSelected?.Invoke();
        }
        public void Dispose()
        {
            _playerInput.mouseLeftUp-=OnMouseLeftUp;
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
            _playerInput.mousePositionChanged-=OnMouseMoved;
        }

        public void Enable()
        {
            _selectedCells = new List<Vector2Int>();

            _playerInput.mouseLeftUp+=OnMouseLeftUp;
            _playerInput.mouseLeftClicked+=OnMouseLeftDown;
            _playerInput.mousePositionChanged+=OnMouseMoved;
        }

        public void Disable()
        {
            _playerInput.mouseLeftUp-=OnMouseLeftUp;
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
            _playerInput.mousePositionChanged-=OnMouseMoved;
        }
    }

}