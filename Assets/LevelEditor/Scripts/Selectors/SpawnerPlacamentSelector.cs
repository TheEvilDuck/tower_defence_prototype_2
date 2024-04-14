using System;
using System.Collections.Generic;
using Services.PlayerInput;
using UnityEngine;
using Grid = Levels.Logic.Grid;


namespace LevelEditor.Selectors
{
    public class SpawnerPlacamentSelector : ISelector
    {
        public event Action<Vector2Int, bool> selectedCellsChanged;
        public event Action cellsSelected;
        public event Action<Vector2Int> selectionStarted;

        private readonly Grid _grid;
        private readonly PlayerInput _playerInput;

        public SpawnerPlacamentSelector(PlayerInput playerInput, Grid grid)
        {
            _playerInput = playerInput;
            _grid = grid;
        }

        public void Disable()
        {
            _playerInput.mouseLeftClicked-=OnMouseLeftDown;
        }

        public void Enable()
        {
            _playerInput.mouseLeftClicked+=OnMouseLeftDown;

            
        }

        private void OnMouseLeftDown(Vector2 position)
        {
            Vector2Int cellPosition = _grid.WorldPositionToGridPosition(position);
            cellPosition.Clamp(new Vector2Int(0,0), new Vector2Int(_grid.GridSize - 1, _grid.GridSize - 1));

            List<Vector2Int> borderPositions = new List<Vector2Int>()
            {
                new Vector2Int(0, cellPosition.y),
                new Vector2Int(_grid.GridSize - 1, cellPosition.y),
                new Vector2Int(cellPosition.x, 0),
                new Vector2Int(cellPosition.x, _grid.GridSize - 1)
            };

            int minDistancePositionId = 0;

            for (int i = 0; i < borderPositions.Count; i++)
            {
                if (Vector2.Distance(_grid.GridPositionToWorldPosition(borderPositions[i]), position) <= 
                Vector2.Distance(_grid.GridPositionToWorldPosition(borderPositions[minDistancePositionId]), position))
                {
                    minDistancePositionId = i;
                }
            }

            if (!_grid.IsCellAt(borderPositions[minDistancePositionId]))
                return;

            Vector2Int result = borderPositions[minDistancePositionId];

            if (result.x == result.y)
            {
                if (result.x == 0)
                    result.x = 1;
                else if (result.x == _grid.GridSize - 1)
                    result.x = _grid.GridSize - 2;
            }

            if (result.x == _grid.GridSize - 1 - result.y)
            {
                if (result.x == 0)
                    result.x = 1;
                else if (result.x == _grid.GridSize - 1)
                    result.x = _grid.GridSize - 2;
            }

            selectionStarted?.Invoke(result);
            cellsSelected?.Invoke();
        }
    }
}
