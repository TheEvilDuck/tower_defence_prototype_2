using System;
using UnityEngine;

namespace Levels.Logic
{
    public class Grid
    {
        private Cell[,] _cells;

        private float _cellSize;

        public event Action<Vector2Int,Cell> cellChanged;
        public event Action<Vector2Int>cellAdded;
        public event Action<Vector2Int> cellRemoved;

        public Grid(int gridSize, float cellSize)
        {
            _cells = new Cell[gridSize,gridSize];
            _cellSize = cellSize;
        }

        public Vector2Int WorldPositionToGridPosition(Vector2 worldPosition)
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

        public bool CreateCellAt(Vector2Int position)
        {
            if (position.x<0||position.y<0||position.x>=_cells.GetLength(0)||position.y>=_cells.GetLength(1))
                return false;

            if (_cells[position.x,position.y]!=null)
                return false;

            Cell cell= new Cell();

            cellAdded?.Invoke(position);

            cell.cellChanged+= () =>
            {
                cellChanged?.Invoke(position,cell);
            };

            _cells[position.x,position.y] = cell;

            return true;
        }

        public void BuildRoadAt(Vector2Int position)
        {
            if (position.x<0||position.y<0||position.x>=_cells.GetLength(0)||position.y>=_cells.GetLength(1))
                return;

            if (_cells[position.x,position.y]!=null)
                return;

            _cells[position.x,position.y].BuildRoad();
        }

        public void RemoveRoadAt(Vector2Int position)
        {
            if (position.x<0||position.y<0||position.x>=_cells.GetLength(0)||position.y>=_cells.GetLength(1))
                return;

            if (_cells[position.x,position.y]!=null)
                return;

            _cells[position.x,position.y].RemoveRoad();
        }

        public bool RemoveCellAt(Vector2Int position)
        {
            if (position.x<0||position.y<0||position.x>=_cells.GetLength(0)||position.y>=_cells.GetLength(1))
                return false;

            if (_cells[position.x,position.y]==null)
                return false;

            _cells[position.x,position.y] = null;

            cellRemoved?.Invoke(position);

            return true;
        }
    }
}
