using System;
using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Levels.Logic;
using Towers;
using Towers.Configs;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace Builder
{
    public class PlacableBuilder: IMainBuilderProvider, IPausable, IDisposable
    {
        public event Action<Vector2Int> mainBuildingBuilt;
        public event Func<Vector2Int, bool> checkCanBuildMainBuilding;
        public event Func<PlacableEnum, bool> checkCanBuild;
        public event Action<PlacableEnum, Vector2Int> placableBuilt;
        public event Action<PlacableEnum, Vector2Int> placableDestroyed;
        public event Action<PlacableEnum> placableBuildStarted;
        public event Action<PlacableEnum> placableBuildCanceled;
        private List<PlacableEnum> _availableTowers;
        private PlacableEnum _currentId;
        private bool _waitForBuilding = false;
        private PlacableFactory _placableFactory;
        private bool _mainBuildingBuilt = false;
        private bool _paused = false;
        private Dictionary<InConstruction, InConstructionData> _inConstructions;
        private List<InConstruction> _markedToDeleteInConstructions;
        private PlacablePreview _preview;
        private GameObjectIconProvider<PlacableEnum> _icons;
        private PlacablesContainer _placables;

        public Placable MainBuilding {get; private set;}
        public bool PreviewAble => _preview.gameObject.activeInHierarchy;

        public PlacableBuilder
        (
            PlacableEnum[] availablePlacables, 
            PlacableFactory placableFactory, 
            bool waitForBuilding, 
            PlacablePreview previewPrefab, 
            GameObjectIconProvider<PlacableEnum> icons,
            PlacablesContainer placablesContainer
        )
        {
            _waitForBuilding = waitForBuilding;
            _availableTowers = new List<PlacableEnum>(availablePlacables);
            _placableFactory = placableFactory;
            _inConstructions = new Dictionary<InConstruction, InConstructionData>();
            _markedToDeleteInConstructions = new List<InConstruction>();
            _icons = icons;
            _placables = placablesContainer;

            _preview = GameObject.Instantiate(previewPrefab);
            DisablePreview();
        }

        public void Dispose()
        {
            if (_preview != null)
                GameObject.Destroy(_preview.gameObject);
        }

        public void Pause()
        {
            foreach (var keyValuePair in _inConstructions)
            {
                keyValuePair.Key.Pause();
            }
        }

        public void UnPause()
        {
            foreach (var keyValuePair in _inConstructions)
            {
                keyValuePair.Key.UnPause();
            }
        }


        public void SwitchCurrentId(PlacableEnum id)
        {
            if (!_availableTowers.Contains(id))
                throw new ArgumentException($"This tower is not available: {id}");

            _currentId = id;
            _preview.UpdatePreview(_icons.Get(id));
        }

        public void EnablePreview() => _preview?.gameObject.SetActive(true);

        public void DisablePreview() => _preview?.gameObject.SetActive(false);

        public void MovePreview(Vector2 position)
        {
            if (_preview == null)
                return;

            _preview.transform.position = position;
        }

        public void DeleteInConstructionAt(Vector2Int cellPosition)
        {
            foreach (var keyValuePair in _inConstructions)
            {
                if (keyValuePair.Value.CellPosition == cellPosition)
                {
                    placableBuildCanceled?.Invoke(keyValuePair.Value.PlacableId);
                    keyValuePair.Key.Cancel();
                    return;
                }
            }
        }

        public void Update()
        {
            if (_paused)
                return;

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

        public void BuildFromPlacableDatas(PlacableData[] placableDatas, Grid grid)
        {
            foreach (var placableData in placableDatas)
            {
                CreatePlacable(grid.ConvertIntToVector2Int(placableData.index), placableData.placable, grid, true);

            }
        }

        public bool Build(Vector2 position, Grid grid)
        {
            if (_availableTowers.Count==0)
                throw new Exception("There are no available towers???");

            Vector2Int cellPosition = grid.WorldPositionToGridPosition(position);

            if (IsInConstructionAt(cellPosition))
            {
                _preview.CantBuildAnimation();
                return false;
            }

            if (!grid.IsCellAt(cellPosition))
            {
                _preview.CantBuildAnimation();
                return false;
            }

            if (!_placables.CanBuildAt(cellPosition))
            {
                _preview.CantBuildAnimation();
                return false;
            }

            bool? canBuild = checkCanBuild?.Invoke(_currentId);

                if (canBuild !=null)
                    if (!(bool)canBuild)
                    {
                        _preview.CantBuildAnimation();
                        return false;
                    }

            if (_currentId==PlacableEnum.MainBuilding)
            {
                if (_mainBuildingBuilt)
                {
                    _preview.CantBuildAnimation();
                    return false;
                }

                bool? canBuildMainBuilding = checkCanBuildMainBuilding?.Invoke(cellPosition);

                if (canBuildMainBuilding !=null)
                    if (!(bool)canBuildMainBuilding)
                    {
                        _preview.CantBuildAnimation();
                        return false;
                    }
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

                placableBuildStarted?.Invoke(_currentId);


            }
            else
            {
                CreatePlacable(cellPosition, _currentId, grid);
            }

            return true;

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

            CreatePlacable(_inConstructions[inConstruction].CellPosition, _inConstructions[inConstruction].PlacableId, _inConstructions[inConstruction].Grid);
        }

        private void CreatePlacable(Vector2Int cellPosition, PlacableEnum placableId, Grid grid, bool delayedBuild = false)
        {
            Placable tower = _placableFactory.Get(placableId);
            Vector2 worldPosition = grid.GridPositionToWorldPosition(cellPosition);
            tower.transform.position = worldPosition;

            if (!_placables.TryBuildAt(cellPosition, tower))
            {
                tower.DestroyPlacable();
                throw new Exception("Didn't you forget to delete inConstrucion placable?");
            }

            if (!delayedBuild)
                tower?.OnBuild();
            
            Action<Placable> placableDestroyedTemp = null;

            placableDestroyedTemp = (Placable placable) =>
            {
                if (placableId == PlacableEnum.MainBuilding)
                    _mainBuildingBuilt = false;

                placable.destroyed -= placableDestroyedTemp;
                placableDestroyed?.Invoke(placableId, cellPosition);
            };

            tower.destroyed += placableDestroyedTemp;

            if (placableId==PlacableEnum.MainBuilding)
            {
                MainBuilding = tower;
                _mainBuildingBuilt = true;
                mainBuildingBuilt?.Invoke(cellPosition);
            }

            placableBuilt?.Invoke(placableId, cellPosition);
        }

        private struct InConstructionData
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
