using System;
using System.Collections.Generic;
using Enemies.AI;
using UnityEngine;

namespace GamePlay
{
    public class Spawners
    {
        private readonly Vector2Int[] _positions;
        private readonly PathFinder _pathFinder;
        private List<Vector2Int> _availablePositions;
        private Dictionary<Vector2Int, List<Vector2Int>> _paths;

        public Spawners(Vector2Int[] positions, PathFinder pathFinder)
        {
            _positions = positions;
            _pathFinder = pathFinder;

            _paths = new Dictionary<Vector2Int, List<Vector2Int>>();
            _availablePositions = new List<Vector2Int>();
        }

        public void CalculateAvailablePositions(Vector2Int mainBuildingPosition, bool useDiagonal, int gridSize)
        {
            _paths.Clear();
            _availablePositions.Clear();

            foreach (Vector2Int spawnerPosition in _positions)
            {
                Vector2Int enemySpawnPosition = spawnerPosition;

                if (enemySpawnPosition.x < 0)
                    enemySpawnPosition.x = 0;

                if(enemySpawnPosition.x >= gridSize)
                    enemySpawnPosition.x = gridSize -1;

                if (enemySpawnPosition.y < 0)
                    enemySpawnPosition.y = 0;

                if(enemySpawnPosition.y >= gridSize)
                    enemySpawnPosition.y = gridSize -1;

                if (_pathFinder.TryFindPath(enemySpawnPosition, mainBuildingPosition, out List<Vector2Int> result, useDiagonal))
                {
                    _availablePositions.Add(enemySpawnPosition);
                    _paths.Add(enemySpawnPosition, result);
                }
            }
        }

        public Vector2Int GetRandomSpawnerPosition()
        {
            return _availablePositions[UnityEngine.Random.Range(0, _availablePositions.Count)];
        }

        public List<Vector2Int> GetPathFrom(Vector2Int spawnerPosition)
        {
            if (!_paths.ContainsKey(spawnerPosition))
                throw new ArgumentException($"Spawner position {spawnerPosition} is not available for path finding!");

            return _paths[spawnerPosition];
        }
    }
}