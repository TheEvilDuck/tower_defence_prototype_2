using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Levels.Logic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Grid = Levels.Logic.Grid;

namespace Enemies.AI
{
    public class PathFinder
    {
        private const int STRAIGHT_PATH_WEIGHT = 10;
        private const int DIAGONAL_PATH_WEIGHT = 14;
        public static Vector2Int[] StraightDirections = 
        {
            new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(0,-1), new Vector2Int(-1,0),
        };

        public static Vector2Int[] DiagonalDirections = 
        {
            new Vector2Int(-1,1), new Vector2Int(1,-1), new Vector2Int(-1,-1), new Vector2Int(1,1)
        };
        private Dictionary<Vector2Int,PathNode> _gridPathDatas;
        private List<Vector2Int> _openList;
        public PathFinder(Grid grid, PathFindMultipliersDatabase pathFindMultipliersDatabase)
        {
            _gridPathDatas = new Dictionary<Vector2Int, PathNode>();
            _openList = new List<Vector2Int>();

            for (int y = 0; y<grid.GridSize;y++)
            {
                for (int x = 0;x<grid.GridSize;x++)
                {
                    Vector2Int position = new Vector2Int(x,y);

                    float costMultiplier = 1f;
                    bool validCell = false;
                    
                    if (grid.TryGetCellDataAt(position, out CellData cellData))
                    {
                        if (cellData.HasRoad)
                        {
                            validCell = pathFindMultipliersDatabase.RoadTileConfig.Walkable;
                            costMultiplier = pathFindMultipliersDatabase.RoadTileConfig.WeightMultiplier;
                        }
                        else if (pathFindMultipliersDatabase.TryGetValue(cellData.Type, out PathFindTileConfig config))
                        {
                            validCell = config.Walkable;
                            costMultiplier = config.WeightMultiplier;
                        }
                    }
                    


                    PathNode pathNode = new PathNode(position, costMultiplier);
                    pathNode.Valid = validCell;
                    _gridPathDatas.Add(position,pathNode);

                    
                }
            }
        }

        public bool TryFindPath(Vector2Int fromPosition, Vector2Int toPosition, out List<Vector2Int> result, bool useDiagonal = false)
        {
            _openList.Clear();
            result = new List<Vector2Int>();

            Debug.Log($"Trying find path from {fromPosition} to {toPosition}");

            if (!_gridPathDatas.ContainsKey(fromPosition))
            {
                Debug.Log("Grid path datas doesn't contain start position");
                return false;
            }

            if (!_gridPathDatas[fromPosition].Valid)
            {
                Debug.Log("Start position is not valid");
                return false;
            }

            if (!_gridPathDatas.ContainsKey(toPosition))
            {
                Debug.Log("Grid path datas doesn't contain end position");
                return false;
            }

            if (!_gridPathDatas[toPosition].Valid)
            {
                Debug.Log("End position is not valid");
                return false;
            }

            foreach (var keyValuePair in _gridPathDatas)
            {
                keyValuePair.Value.PreviousNode = null;
                keyValuePair.Value.DistanceFromStart = int.MaxValue;
                keyValuePair.Value.DistanceToTarget = 0;
                keyValuePair.Value.CalculateWeight();
                keyValuePair.Value.IsClosed = false;
            }

            _openList.Add(fromPosition);
            _gridPathDatas[fromPosition].DistanceFromStart = 0;
            _gridPathDatas[fromPosition].DistanceToTarget = GetManhatanDistanceBetween(fromPosition,toPosition);
            _gridPathDatas[fromPosition].CalculateWeight();

            while (_openList.Count > 0)
            {
                Vector2Int currentPosition = GetBestNextPositionFromOpenList();

                if (currentPosition == toPosition)
                {
                    result = GenerateResultPath(currentPosition);
                    return true;
                }

                _gridPathDatas[currentPosition].IsClosed = true;
                _openList.Remove(currentPosition);

                CheckPositionsAround(currentPosition, useDiagonal, toPosition);
            }

            return false;
        }

        private void CheckPositionsAround(Vector2Int position, bool useDiagonal, Vector2Int targetPosition)
        {
            HandleOffsets(StraightDirections, position, targetPosition);
            
            if (useDiagonal)
                HandleOffsets(DiagonalDirections, position, targetPosition);
        }

        private void HandleOffsets(Vector2Int[] offsets,Vector2Int position, Vector2Int targetPosition)
        {
            foreach (Vector2Int offset in offsets)
            {
                Vector2Int positionWithOffset = position+offset;

                if (!_gridPathDatas.ContainsKey(positionWithOffset))
                    continue;

                if (!_gridPathDatas[positionWithOffset].Valid)
                    continue;

                if (_gridPathDatas[positionWithOffset].IsClosed)
                    continue;

                int manhatanDistance = GetManhatanDistanceBetween(position,positionWithOffset);
                float tileTypeBasedCost = manhatanDistance*_gridPathDatas[positionWithOffset].CoctMultiplier;
                int tentativeDistance = _gridPathDatas[position].DistanceFromStart+(int)tileTypeBasedCost;

                if (tentativeDistance < _gridPathDatas[positionWithOffset].DistanceFromStart)
                {
                    _gridPathDatas[positionWithOffset].PreviousNode = _gridPathDatas[position];
                    _gridPathDatas[positionWithOffset].DistanceFromStart = tentativeDistance;
                    _gridPathDatas[positionWithOffset].DistanceToTarget = GetManhatanDistanceBetween(positionWithOffset, targetPosition);
                    _gridPathDatas[positionWithOffset].CalculateWeight();

                    if (!_openList.Contains(positionWithOffset))
                        _openList.Add(positionWithOffset);
                }
            }
        }

        private Vector2Int GetBestNextPositionFromOpenList()
        {
            Vector2Int result = _openList[0];

            foreach (Vector2Int position in _openList)
            {
                if 
                (
                    _gridPathDatas[position].Weight<_gridPathDatas[result].Weight||
                    _gridPathDatas[position].Weight==_gridPathDatas[result].Weight&&
                    _gridPathDatas[position].DistanceToTarget<_gridPathDatas[result].DistanceToTarget
                )

                    result = position;
            }

            return result;
        }

        private List<Vector2Int> GenerateResultPath(Vector2Int lastPosition)
        {
            List<Vector2Int> result = new List<Vector2Int>();

            Vector2Int currentPosition = lastPosition;

            while (_gridPathDatas[currentPosition].PreviousNode!=null)
            {
                result.Add(currentPosition);
                currentPosition = _gridPathDatas[currentPosition].PreviousNode.Position;
            }

            result.Reverse();

            return result;
        }

        private int GetManhatanDistanceBetween(Vector2Int from, Vector2Int to)
        {
            int xDistance = Mathf.Abs(from.x - to.x);
            int yDistance = Mathf.Abs(from.y - to.y);

            int remaining = Mathf.Abs(xDistance-yDistance);

            return DIAGONAL_PATH_WEIGHT * Mathf.Min(xDistance,yDistance) + STRAIGHT_PATH_WEIGHT * remaining;
        }
        

        private class PathNode
        {
            public PathNode PreviousNode {get; set;}
            public int DistanceToTarget {get; set;}
            public int DistanceFromStart {get; set;}
            public int Weight {get; private set;}
            public bool IsClosed {get; set;}
            public bool Valid {get; set;}
            public Vector2Int Position {get; private set;}
            public float CoctMultiplier {get; private set;} = 1f;

            public PathNode(Vector2Int position, float costMultiplier)
            {
                Position = position;

                if (costMultiplier <= 0 )
                    throw new ArgumentOutOfRangeException("Cost multiplier for nodes must not be less or equal 0");

                CoctMultiplier = costMultiplier;
            }

            public void CalculateWeight()
            {
                Weight = DistanceFromStart+DistanceToTarget;
            }
        }
    }
}
