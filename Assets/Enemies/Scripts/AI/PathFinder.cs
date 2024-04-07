using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Levels.Logic;
using UnityEngine;
using UnityEngine.Pool;
using Grid = Levels.Logic.Grid;

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
    private Queue<Vector2Int> _openList;
    public PathFinder(Grid grid)
    {
        _gridPathDatas = new Dictionary<Vector2Int, PathNode>();
        _openList = new Queue<Vector2Int>();

        for (int y = 0; y<grid.GridSize;y++)
        {
            for (int x = 0;x<grid.GridSize;x++)
            {
                Vector2Int position = new Vector2Int(x,y);
                PathNode pathNode = new PathNode(position);

                _gridPathDatas.Add(position,pathNode);

                pathNode.Valid  = grid.IsCellAt(position);
            }
        }
    }

    public bool TryFindPath(Vector2Int fromPosition, Vector2Int toPosition, out List<Vector2Int> result, bool useDiagonal = false)
    {
        _openList.Clear();

        result = new List<Vector2Int>();

        if (!_gridPathDatas.ContainsKey(toPosition))
            return false;

        if (!_gridPathDatas[toPosition].Valid)
            return false;

        if (!_gridPathDatas.ContainsKey(fromPosition))
            return false;

        if (!_gridPathDatas[fromPosition].Valid)
            return false;

        _openList.Enqueue(fromPosition);

        foreach (KeyValuePair<Vector2Int,PathNode> keyValuePair in _gridPathDatas)
        {
            keyValuePair.Value.IsClosed = false;
            keyValuePair.Value.DistanceFromStart = 0;
            keyValuePair.Value.DistanceToTarget = 0;
            keyValuePair.Value.Weight = 0;
            keyValuePair.Value.PreviousNode = null;
        }

        while (_openList.Count>0)
        {
            Vector2Int currentPosition = _openList.Dequeue();
            _gridPathDatas[currentPosition].IsClosed = true;

            if (currentPosition==toPosition)
            {
                while (_gridPathDatas[currentPosition].PreviousNode!=null)
                {
                    result.Add(currentPosition);
                    currentPosition = _gridPathDatas[currentPosition].PreviousNode.Position;
                }

                result.Reverse();
                return true;
            }

            CheckPositionsAround(currentPosition, useDiagonal, toPosition);
        }

        return false;
    }

    private void CheckPositionsAround(Vector2Int position, bool useDiagonal, Vector2Int targetPosition)
    {
        HandleOffsets(StraightDirections, STRAIGHT_PATH_WEIGHT, position, targetPosition);
        
        if (useDiagonal)
            HandleOffsets(DiagonalDirections, DIAGONAL_PATH_WEIGHT, position, targetPosition);
    }

    private void HandleOffsets(Vector2Int[] offsets,int pathWeight, Vector2Int position, Vector2Int targetPosition)
    {
        foreach (Vector2Int offset in offsets)
        {
            if (!_gridPathDatas.ContainsKey(position+offset))
                continue;

            if (_gridPathDatas[position+offset].IsClosed||!_gridPathDatas[position+offset].Valid)
                continue;

            //PathNode neightBor = new PathNode(cellData, cellData.Position+offset, pathWeight);
            PathNode neightBor = _gridPathDatas[position+offset];
            neightBor.PreviousNode = _gridPathDatas[position];
            neightBor.DistanceFromStart = _gridPathDatas[position].DistanceFromStart+pathWeight;

            int weight = _gridPathDatas[position].DistanceFromStart+GetManhatanDistanceBetween(position, position+offset);

            if (weight < neightBor.DistanceFromStart||!_openList.Contains(position+offset))
            {
                neightBor.DistanceFromStart = weight;
                neightBor.DistanceToTarget = GetManhatanDistanceBetween(position+offset, targetPosition);

                if (!_openList.Contains(position+offset))
                    _openList.Enqueue(position+offset);
            }
        }
    }

    private int GetManhatanDistanceBetween(Vector2Int from, Vector2Int to)
    {
        /*int deltaX = Mathf.Abs(from.x - to.x);
        int deltaY = Mathf.Abs(from.y - to.y);

        if (deltaX > deltaY)
            return DIAGONAL_PATH_WEIGHT * deltaY + STRAIGHT_PATH_WEIGHT * (deltaX - deltaY);
        return DIAGONAL_PATH_WEIGHT * deltaX + STRAIGHT_PATH_WEIGHT * (deltaY - deltaX);*/

        return (int)(Vector2Int.Distance(from, to)*10f);
    }
    

    private class PathNode
    {
        public PathNode PreviousNode {get; set;}
        public int DistanceToTarget {get; set;}
        public int DistanceFromStart {get; set;}
        public int Weight {get; set;}
        public bool IsClosed {get; set;}
        public bool Valid {get; set;}
        public Vector2Int Position {get; private set;}

        public PathNode(Vector2Int position)
        {
            Position = position;
        }
    }
}
