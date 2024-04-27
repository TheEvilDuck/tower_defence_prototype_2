using System;
using System.Collections.Generic;
using Levels.Tiles;
using UnityEngine;

namespace Levels.Logic
{
    public class Grid: IGridCellRemovedNotifier
    {
        private CellData[,] _cells;
        private float _cellSize;

        public event Action<Vector2Int,CellData> cellChanged;
        public event Action<Vector2Int, TileType>cellAdded;
        public event Action<Vector2Int> cellRemoved;

        public float CellSize => _cellSize;

        public int GridSize => _cells.GetLength(0);

        public Grid(int gridSize, float cellSize)
        {
            _cells = new CellData[gridSize,gridSize];

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    _cells[x, y] = new CellData
                    {
                        Type = TileType.Empty
                    };
                }
            }
            
            _cellSize = cellSize;
        }

        public void FillFromGridData(GridData gridData)
        {
            Clear();

            _cells = new CellData[gridData.gridSize,gridData.gridSize];

            for (int x = 0; x < GridSize; x++)
            {
                for (int y = 0; y < GridSize; y++)
                {
                    _cells[x, y] = new CellData
                    {
                        Type = TileType.Empty
                    };
                }
            }

            for (int i = 0; i < gridData.cells.Length; i+=2)
            {
                CreateCellAt(ConvertIntToVector2Int(gridData.cells[i]), (TileType)gridData.cells[i+1]);
            }

            foreach(int roadIndex in gridData.roadIndexes)
                BuildRoadAt(ConvertIntToVector2Int(roadIndex));
        }

        public void Clear()
        {
            for (int x = 0;x<_cells.GetLength(0);x++)
            {
                for (int y = 0; y< _cells.GetLength(1); y++)
                {
                    RemoveCellAt(new Vector2Int(x,y));
                }
            }
        }

        public bool HasRoadAt(Vector2Int position)
        {
            if (IsCellAt(position))
                return _cells[position.x,position.y].HasRoad;

            return false;
        }

        public bool TryGetCellDataAt(Vector2Int position, out CellData cellData)
        {
            cellData = new CellData();

            if (!IsCellAt(position))
                return false;

            cellData = _cells[position.x, position.y];

            return true;
        }

        public int ConvertVector2IntToIndex(Vector2Int cellPosition) => (cellPosition.x*GridSize)+cellPosition.y;

        public GridData ConvertToGridData()
        {
            GridData gridData = new GridData
            {
                gridSize = _cells.GetLength(0)
            };

            List<int>cells= new List<int>();
            List<int>roadIndexes = new List<int>();

            for (int x = 0;x<gridData.gridSize;x++)
            {
                for (int y = 0; y< gridData.gridSize; y++)
                {
                    if (_cells[x,y].Type != TileType.Empty)
                    {
                        int index = ConvertVector2IntToIndex(new Vector2Int(x,y));
                        cells.Add(index);
                        int tileType = (int)_cells[x,y].Type;
                        cells.Add(tileType);

                        if (_cells[x,y].HasRoad)
                            roadIndexes.Add(index);
                    }
                }
            }

            gridData.cells= cells.ToArray();
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

            return _cells[position.x,position.y].Type != TileType.Empty;
        }

        public bool CreateCellAt(Vector2Int position, TileType tileType)
        {
            if (!IsPositionValid(position))
                return false;

            if (IsCellAt(position))
                return false;

            _cells[position.x,position.y].Type = tileType;
            cellAdded?.Invoke(position, tileType);
            
            return true;
        }

        public void BuildRoadAt(Vector2Int position)
        {
            if (!IsCellAt(position))
                return;

            _cells[position.x,position.y].HasRoad = true;

            cellChanged?.Invoke(position, _cells[position.x,position.y]);
        }

        public void RemoveRoadAt(Vector2Int position)
        {
            if (!IsCellAt(position))
                return;

            _cells[position.x,position.y].HasRoad = false;

            cellChanged?.Invoke(position, _cells[position.x,position.y]);
        }

        public bool RemoveCellAt(Vector2Int position)
        {
            if (!IsPositionValid(position))
                return false;

            if (_cells[position.x,position.y].Type == TileType.Empty)
                return false;

            if (_cells[position.x,position.y].HasRoad)
                RemoveRoadAt(position);

            _cells[position.x,position.y].Type = TileType.Empty;

            cellRemoved?.Invoke(position);

            return true;
        }
        public Vector2Int ConvertIntToVector2Int(int index)
        {
            int x = (index / GridSize);
            int y = (index % GridSize);

            return new Vector2Int(x,y);
        }
    }
}
