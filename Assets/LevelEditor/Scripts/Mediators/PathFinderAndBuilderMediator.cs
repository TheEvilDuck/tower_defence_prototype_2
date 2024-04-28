using System;
using Builder;
using Enemies.AI;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Mediators
{
    public class PathFinderAndBuilderMediator : IDisposable
    {
        private readonly PathFinder _pathFinder;
        private readonly PlacableBuilder _placableBuilder;
        private readonly SpawnerPositions _spawnerPositions;

        public PathFinderAndBuilderMediator(PathFinder pathFinder, PlacableBuilder placableBuilder,SpawnerPositions spawnerPositions)
        {
            _pathFinder = pathFinder;
            _placableBuilder = placableBuilder;
            _spawnerPositions = spawnerPositions;

            _placableBuilder.checkCanBuildMainBuilding += CheckMainBuildingPlacementPossibility;
        }
        public void Dispose()
        {
            _placableBuilder.checkCanBuildMainBuilding -= CheckMainBuildingPlacementPossibility;
        }

        private bool CheckMainBuildingPlacementPossibility(Vector2Int position)
        {
            bool can = false;

            foreach (var spawnerPosition in _spawnerPositions.Spawners)
            {
                can = _pathFinder.TryFindPath(spawnerPosition, position, out var result, false);

                if (can)
                    break;
            }

            return can;
        }
    }
}