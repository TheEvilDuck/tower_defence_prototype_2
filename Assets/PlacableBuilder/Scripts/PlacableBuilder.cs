using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace Builder
{
    public class PlacableBuilder
    {
        public event Action mainBuildingBuilt;
        private List<PlacableEnum> _availableTowers;
        private Placable _inConstructionPrefab;
        private PlacableEnum _currentId;
        private bool _waitForBuilding = false;
        private PlacableFactory _placableFactory;
        private bool _mainBuildingBuilt = false;
        
        public PlacableBuilder(AvailablePlacables availablePlacables, PlacableFactory placableFactory, Placable inConstructionPrefab): this(availablePlacables,placableFactory)
        {
            _inConstructionPrefab = inConstructionPrefab;
            _waitForBuilding = true;
        }

        public PlacableBuilder(AvailablePlacables availablePlacables, PlacableFactory placableFactory)
        {
            _availableTowers = new List<PlacableEnum>(availablePlacables.placableIds);
            _placableFactory = placableFactory;
        }

        public void SwitchCurrentId(PlacableEnum id)
        {
            if (!_availableTowers.Contains(id))
                throw new ArgumentException($"This tower is not available: {id}");

            _currentId = id;
        }

        public void Build(Vector2 position, Grid grid)
        {
            if (_availableTowers.Count==0)
                throw new Exception("There are no available towers???");

            Vector2Int cellPosition = grid.WorldPositionToGridPosition(position);

            if (!grid.CanBuildAt(cellPosition))
                throw new Exception("Didn't you forget to delete inConstrucion placable?");

            if (_currentId==PlacableEnum.MainBuilding&&_mainBuildingBuilt)
                return;

            Placable tower = _placableFactory.Get(_currentId);
            Vector2 worldPosition = grid.GridPositionToWorldPosition(cellPosition);
            tower.transform.position = worldPosition;

            if (!grid.TryBuildAt(cellPosition, tower))
            {
                tower.DestroyPlacable();
                throw new Exception("Didn't you forget to delete inConstrucion placable?");
            }

            tower?.OnBuild();

            if (_currentId==PlacableEnum.MainBuilding)
            {
                _mainBuildingBuilt = true;
                mainBuildingBuilt?.Invoke();
            }
        }
    }
}
