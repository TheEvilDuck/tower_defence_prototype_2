using System;
using UnityEngine;

namespace Levels.Logic
{
    public class Grid
    {
        private Cell[,] _cells;

        private float _cellSize;

        public event Action<Vector2Int> cellChanged;

        public Grid(int gridSize, float cellSize)
        {
            _cells = new Cell[gridSize,gridSize];
            _cellSize = cellSize;
        }

        public Vector2Int WorldPositionToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x/_cellSize);
            int y = Mathf.FloorToInt(worldPosition.y/_cellSize);
            return new Vector2Int(x,y);
        }
        public Vector2 GridPositionToWorldPosition(Vector2Int gridPosition)
        {
            float x = gridPosition.x*_cellSize+_cellSize/2f;
            float y = gridPosition.y*_cellSize+_cellSize/2f;
            return new Vector2(x,y);
        }

        public void CreateCellAt(Vector2Int position)
        {
            _cells[position.x,position.y] = new Cell();

            cellChanged?.Invoke(position);
        }
    }
}
