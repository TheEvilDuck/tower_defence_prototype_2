using System;
using System.Collections.Generic;
using Towers;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels.Logic
{
    public class Grid: IDisposable
    {
        private Cell[,] _cells;

        private float _cellSize;

        public event Action<Vector2Int,Cell> cellChanged;
        public event Action<Vector2Int>cellAdded;
        public event Action<Vector2Int> cellRemoved;

        public float CellSize => _cellSize;

        public int GridSize => _cells.GetLength(0);

        public Grid(int gridSize, float cellSize)
        {
            _cells = new Cell[gridSize,gridSize];
            _cellSize = cellSize;
        }

        public void FillFromGridData(GridData gridData)
        {
            Clear();

            _cells = new Cell[gridData.gridSize,gridData.gridSize];

            foreach (int index in gridData.cellsIndexes)
                CreateCellAt(ConvertIntToVector2Int(index,gridData.gridSize));

            foreach(int roadIndex in gridData.roadIndexes)
                BuildRoadAt(ConvertIntToVector2Int(roadIndex,gridData.gridSize));
        }

        public void Clear()
        {
            for (int x = 0;x<_cells.GetLength(0);x++)
            {
                for (int y = 0; y< _cells.GetLength(1); y++)
                {
                    if (_cells[x,y]!=null)
                    {
                        RemoveCellAt(new Vector2Int(x,y));
                    }
                }
            }
        }

        public GridData ConvertToGridData()
        {
            GridData gridData = new GridData();
            gridData.gridSize = _cells.GetLength(0);
            
            List<int>cellIndexes = new List<int>();
            List<int>roadIndexes = new List<int>();

            for (int x = 0;x<gridData.gridSize;x++)
            {
                for (int y = 0; y< gridData.gridSize; y++)
                {
                    if (_cells[x,y]!=null)
                    {
                        int index = (x*gridData.gridSize)+y;
                        cellIndexes.Add(index);

                        if (_cells[x,y].HasRoad)
                            roadIndexes.Add(index);
                    }
                }
            }

            gridData.cellsIndexes = cellIndexes.ToArray();
            gridData.roadIndexes = roadIndexes.ToArray();

            return gridData;
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

        public bool IsPositionValid(Vector2Int position)
        {
            return position.x>=0&&position.y>=0&&position.x<_cells.GetLength(0)&&position.y<_cells.GetLength(1);
        }

        public bool IsCellAt(Vector2Int position)
        {
            if (!IsPositionValid(position))
                return false;

            return !(_cells[position.x,position.y]==null);
        }

        public bool CreateCellAt(Vector2Int position)
        {
            if (!IsPositionValid(position))
                return false;

            if (IsCellAt(position))
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
            if (!IsCellAt(position))
                return;

            _cells[position.x,position.y].BuildRoad();
        }

        public void RemoveRoadAt(Vector2Int position)
        {
            if (!IsCellAt(position))
                return;

            _cells[position.x,position.y].RemoveRoad();
        }

        public bool RemoveCellAt(Vector2Int position)
        {
            if (!IsPositionValid(position))
                return false;

            if (_cells[position.x,position.y]==null)
                return false;

            if (_cells[position.x,position.y].HasRoad)
                RemoveRoadAt(position);

            DestroyAt(position);

            _cells[position.x,position.y].Dispose();
            _cells[position.x,position.y] = null;

            cellRemoved?.Invoke(position);

            return true;
        }

        public void Dispose()
        {
            foreach (Cell cell in _cells)
                if (cell!=null)
                    cell.Dispose();
        }

        public bool CanBuildAt(Vector2Int position)
        {
            if (!IsCellAt(position))
                return false;

            if (_cells[position.x,position.y].Placable != null)
                return false;

            return true;
        }

        public bool TryBuildAt(Vector2Int position, Placable placable)
        {
            if (!CanBuildAt(position))
                return false;

            return _cells[position.x,position.y].TryPlace(placable);
        }

        public void DestroyAt(Vector2Int position)
        {
             if (!IsCellAt(position))
                return;

            if (_cells[position.x,position.y].Placable == null)
                return;

            _cells[position.x,position.y].TryDestroyPlacable();
        }

        private Vector2Int ConvertIntToVector2Int(int index, int gridSize)
        {
            int x = (index/gridSize);
            int y = (index%gridSize);

            return new Vector2Int(x,y);
        }
    }
}
