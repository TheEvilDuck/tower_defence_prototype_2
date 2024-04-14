using System;
using Builder;
using Enemies.AI;
using UnityEngine;

namespace GamePlay
{
    public class SpawnerMediator: IDisposable
    {
        private EnemySpawner _enemySpawner;
        private PathFinder _pathFinder;
        private PlacableBuilder _placableBuilder;
        private Spawners _spawners;
        private readonly int _gridSize;

        public SpawnerMediator(EnemySpawner enemySpawner, PathFinder pathFinder, PlacableBuilder placableBuilder, Spawners spawners, int gridSize)
        {
            _enemySpawner = enemySpawner;
            _pathFinder = pathFinder;
            _placableBuilder = placableBuilder;
            _spawners = spawners;
            _gridSize = gridSize;
            _placableBuilder.mainBuildingBuilt+=OnMainBuildingBuilt;
        }

        public void Dispose()
        {
            _placableBuilder.mainBuildingBuilt-=OnMainBuildingBuilt;
        }

        private void OnMainBuildingBuilt(Vector2Int cellPosition)
        {
            _spawners.CalculateAvailablePositions(cellPosition, true, _gridSize);
            Debug.Log("PATH FIND END");
            _enemySpawner.Start();
        }
    }
}