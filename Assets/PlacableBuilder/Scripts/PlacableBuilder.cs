using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using Towers;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace Builder
{
    public class PlacableBuilder: IMainBuilderProvider
    {
        public event Action<Vector2Int> mainBuildingBuilt;
        public event Func<Vector2Int, bool> canBuildMainBuilding;
        private List<PlacableEnum> _availableTowers;
        private PlacableEnum _currentId;
        private bool _waitForBuilding = false;
        private PlacableFactory _placableFactory;
        private bool _mainBuildingBuilt = false;
        private Dictionary<InConstruction, InConstructionData> _inConstructions;
        private List<InConstruction> _markedToDeleteInConstructions;

        public Placable MainBuilding {get; private set;}

        public PlacableBuilder(AvailablePlacables availablePlacables, PlacableFactory placableFactory, bool waitForBuilding)
        {
            _waitForBuilding = waitForBuilding;
            _availableTowers = new List<PlacableEnum>(availablePlacables.placableIds);
            _placableFactory = placableFactory;
            _inConstructions = new Dictionary<InConstruction, InConstructionData>();
            _markedToDeleteInConstructions = new List<InConstruction>();
        }

        public void SwitchCurrentId(PlacableEnum id)
        {
            if (!_availableTowers.Contains(id))
                throw new ArgumentException($"This tower is not available: {id}");

            _currentId = id;
        }

        public void DeleteInConstructionAt(Vector2Int cellPosition)
        {
            foreach (var keyValuePair in _inConstructions)
            {
                if (keyValuePair.Value.CellPosition == cellPosition)
                {
                    keyValuePair.Key.Cancel();
                    return;
                }
            }
        }

        public void Update()
        {
            foreach (var keyValuePair in _inConstructions)
            {
                keyValuePair.Key.Update();
            }

            foreach (InConstruction inConstruction in _markedToDeleteInConstructions)
            {
                UnityEngine.GameObject.Destroy(_inConstructions[inConstruction].InConstructionObject);
                _inConstructions.Remove(inConstruction);
            }

            _markedToDeleteInConstructions.Clear();
        }

        public void Build(Vector2 position, Grid grid)
        {
            if (_availableTowers.Count==0)
                throw new Exception("There are no available towers???");

            Vector2Int cellPosition = grid.WorldPositionToGridPosition(position);

            if (IsInConstructionAt(cellPosition))
                return;

            if (!grid.IsCellAt(cellPosition))
                return;

            if (!grid.CanBuildAt(cellPosition))
                throw new Exception("Didn't you forget to delete inConstrucion placable?");

            if (_currentId==PlacableEnum.MainBuilding)
            {
                if (_mainBuildingBuilt)
                    return;

                bool? canBuild = canBuildMainBuilding?.Invoke(cellPosition);

                if (canBuild !=null)
                    if (!(bool)canBuild)
                        return;
            }

            if (_waitForBuilding)
            {
                PlacableConfig config = _placableFactory.GetConfig(_currentId);
                InConstruction inConstruction = new InConstruction(config.BuildTime);
                GameObject inConstructionObject = _placableFactory.GetInConstruction();
                inConstructionObject.transform.position = grid.GridPositionToWorldPosition(cellPosition);
                InConstructionData inConstructionData = new InConstructionData(inConstructionObject, grid, cellPosition, _currentId);
                _inConstructions.Add(inConstruction, inConstructionData);
                inConstruction.end += OnInConstructionEnd;


            }
            else
            {
                CreatePlacable(grid, cellPosition, _currentId);
            }

            
        }

        private bool IsInConstructionAt(Vector2Int cellPosition)
        {
            foreach (var keyValuePair in _inConstructions)
            {
                if (keyValuePair.Value.CellPosition == cellPosition)
                    return true;
            }

            return false;
        }

        private void OnInConstructionEnd(InConstruction inConstruction, bool success)
        {
            inConstruction.end -= OnInConstructionEnd;
            _markedToDeleteInConstructions.Add(inConstruction);

            if (!success)
            {
                return;
            }

            CreatePlacable(_inConstructions[inConstruction].Grid, _inConstructions[inConstruction].CellPosition, _inConstructions[inConstruction].PlacableId);
        }

        private void CreatePlacable(Grid grid, Vector2Int cellPosition, PlacableEnum placableId)
        {
            Debug.Log(placableId);

            Placable tower = _placableFactory.Get(placableId);
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
                MainBuilding = tower;
                _mainBuildingBuilt = true;
                mainBuildingBuilt?.Invoke(cellPosition);
            }
        }

        private class InConstructionData
        {
            public GameObject InConstructionObject {get; private set;}
            public Grid Grid {get; private set;}
            public Vector2Int CellPosition {get; private set;}
            public PlacableEnum PlacableId {get; private set;}

            public InConstructionData(GameObject inConstructionObject, Grid grid, Vector2Int cellPosition, PlacableEnum placableId)
            {
                InConstructionObject = inConstructionObject;
                Grid = grid;
                CellPosition = cellPosition;
                PlacableId = placableId;
            }
        }
    }
}
