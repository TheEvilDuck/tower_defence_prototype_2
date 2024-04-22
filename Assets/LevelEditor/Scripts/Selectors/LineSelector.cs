using System;
using System.Collections.Generic;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Selectors
{
    public class LineSelector : ISelector
    {
        public event Action<Vector2Int,bool> selectedCellsChanged;
        public event Action cellsSelected;
        public event Action<Vector2Int> selectionStarted;

        private PlayerInput _playerInput;
        private Grid _grid;
        private Vector2Int _startPos;
        private Vector2Int _endPos;
        private bool _inProcess;
        private List<Vector2Int>_selectedCells;

        public LineSelector(PlayerInput playerInput, Grid grid)
        {
            _playerInput = playerInput;
            _grid = grid;
            _selectedCells = new List<Vector2Int>();
        }

        public void Disable()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
            _playerInput.mouseLeftHold-=OnMoseLeftMove;
            _playerInput.mouseLeftUp-=OnMouseLeftUp;
            _playerInput.mouseBlocked -= OnMouseBlocked;
        }

        public void Enable()
        {
            _playerInput.mouseLeftClicked+=OnMouseLeftDown;
            _playerInput.mouseLeftHold+=OnMoseLeftMove;
            _playerInput.mouseLeftUp+=OnMouseLeftUp;
            _playerInput.mouseBlocked += OnMouseBlocked;
        }

        private void OnMouseLeftDown(Vector2 mousePosition)
        {
            _endPos = _startPos = _grid.WorldPositionToGridPosition(mousePosition);

            _inProcess = true;

            _selectedCells.Clear();

            _selectedCells.Add(_startPos);
            selectionStarted?.Invoke(_startPos);
        }

        private void OnMoseLeftMove(Vector2 mousePosition)
        {
            if (!_inProcess)
                return;

            if (_playerInput.MouseBlocked)
                OnMouseLeftUp(mousePosition);

            Vector2Int cellPosition = _grid.WorldPositionToGridPosition(mousePosition);

            if (cellPosition==_endPos)
                return;

            if (cellPosition==_startPos)
            {
                if (_startPos==_endPos)
                    return;
            }

            List<Vector2Int>sellectedPositionsToRemove = new List<Vector2Int>();

            if (_endPos!=_startPos)
            {
                foreach (Vector2Int pos in _selectedCells)
                {
                    if (pos!=_startPos)
                        sellectedPositionsToRemove.Add(pos);
                }
            }

            foreach (Vector2Int position in sellectedPositionsToRemove)
            {
                _selectedCells.Remove(position);
                selectedCellsChanged?.Invoke(position,false);
            }
            _endPos = cellPosition;
            
            if (!_selectedCells.Contains(_endPos))
            {
                _selectedCells.Add(_endPos);
                selectedCellsChanged?.Invoke(_endPos,true);

                Vector2 moveVector = _endPos-_startPos;
                Vector2 direction =  moveVector.normalized;

                int multiplier = 1;

                while ((direction*multiplier).magnitude<moveVector.magnitude)
                {
                    Vector2Int pos = new Vector2Int((int)(direction*multiplier).x,(int)(direction*multiplier).y);

                    _selectedCells.Add(_startPos+pos);
                    selectedCellsChanged?.Invoke(_startPos+pos,true);

                    multiplier++;

                }
            }

        }
        private void OnMouseLeftUp(Vector2 mousePosition)
        {
            _inProcess = false;

            cellsSelected?.Invoke();
        }

        private void OnMouseBlocked(bool isBlocked)
        {
            if (isBlocked)
                OnMouseLeftUp(Vector2.zero);
        }

    }
}
