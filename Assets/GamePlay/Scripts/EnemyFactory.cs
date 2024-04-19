using System;
using System.Collections.Generic;
using Common;
using Enemies;
using Enemies.AI;
using Levels.Logic;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace GamePlay
{
    public class EnemyFactory
    {
        private readonly EnemiesDatabase _enemies;
        private readonly Spawners _spawners;
        private readonly Grid _grid;
        private readonly PathFindMultipliersDatabase _pathFindMultiplierDatabase;
        public EnemyFactory(EnemiesDatabase enemies, Grid grid, Spawners spawners, PathFindMultipliersDatabase pathFindMultipliersDatabase)
        {
            _enemies = enemies;
            _grid = grid;
            _spawners = spawners;
            _pathFindMultiplierDatabase = pathFindMultipliersDatabase;
        }
        public Enemy Get(EnemyEnum enemyid)
        {
            if (!_enemies.TryGetValue(enemyid, out EnemyConfig config))
                throw new ArgumentException($"There is no config for {enemyid} in enemies database");

            Enemy enemy = UnityEngine.Object.Instantiate(config.Prefab);
            enemy.Init(config, _grid);
            Vector2Int randomSpawnerLocation = _spawners.GetRandomSpawnerPosition();
            Vector2 position = _grid.GridPositionToWorldPosition(randomSpawnerLocation);
            enemy.transform.position = position;
            UpdateEnemyPath(enemy, _spawners.GetPathFrom(randomSpawnerLocation));
            return enemy;
        }

        private void UpdateEnemyPath(Enemy enemy, List<Vector2Int> pathPositions)
        {
            List<EnemyPathNode> path = new List<EnemyPathNode>();

            foreach (Vector2Int gridPoint in pathPositions)
            {
                Vector2 worldPoint = _grid.GridPositionToWorldPosition(gridPoint);

                if (!_grid.TryGetCellDataAt(gridPoint, out CellData cellData))
                    throw new Exception("Path node returned invalid cell");

                if (!_pathFindMultiplierDatabase.TryGetValue(cellData.Type, out PathFindTileConfig config))
                    throw new ArgumentException($"Did you forget to add {cellData.Type} to path find multiplier database?");

                EnemyPathNode enemyPathNode = new EnemyPathNode()
                {
                    position = worldPoint,
                    speedMultiplier = 1f/config.WeightMultiplier
                            
                };
                path.Add(enemyPathNode);
            }

            enemy.UpdatePath(path);
        }
    }
}
