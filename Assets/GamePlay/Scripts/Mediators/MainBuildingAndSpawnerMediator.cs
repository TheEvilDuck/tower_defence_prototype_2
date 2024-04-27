using Builder;
using GamePlay.EnemiesSpawning;
using UnityEngine;

namespace GamePlay.Mediators
{
    public class MainBuildingAndSpawnerMediator
    {
        private readonly PlacableBuilder _mainBuildingProvider;
        private readonly Spawners _spawners;
        private readonly int _gridSize;

        public MainBuildingAndSpawnerMediator(PlacableBuilder mainBuilderProvider, Spawners spawners, int grisSize)
        {
            _mainBuildingProvider = mainBuilderProvider;
            _spawners = spawners;
            _gridSize = grisSize;

            _mainBuildingProvider.mainBuildingBuilt += OnMainBuildingBuilt;
        }

        private void OnMainBuildingBuilt(Vector2Int position)
        {
            Debug.Log("A");
            _mainBuildingProvider.mainBuildingBuilt -= OnMainBuildingBuilt;
            _spawners.CalculateAvailablePositions(position, false, _gridSize);
        }
    }
}