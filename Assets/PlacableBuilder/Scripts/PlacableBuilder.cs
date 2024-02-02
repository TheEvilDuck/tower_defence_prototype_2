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
        private Dictionary<PlacableEnum,PlacableConfig> _towers;
        private Placable _inConstructionPrefab;
        private PlacableEnum _currentId;
        private bool _waitForBuilding = false;
        public PlacableBuilder(TowersDatabase towersDatabase, AvailablePlacables availablePlacables, Placable inConstructionPrefab): this(towersDatabase,availablePlacables)
        {
            _inConstructionPrefab = inConstructionPrefab;
            _waitForBuilding = true;
        }

        public PlacableBuilder(TowersDatabase towersDatabase, AvailablePlacables availablePlacables)
        {
            _towers = new Dictionary<PlacableEnum, PlacableConfig>();

            foreach (PlacableEnum placableId in availablePlacables.placableIds)
            {
                if (!towersDatabase.TryGetValue(placableId, out PlacableConfig config))
                    throw new ArgumentException($"There is not config for tower presenting {placableId}");

                if (_towers.ContainsKey(placableId))
                    Debug.LogError($"You're trying to add dublicate to builder? {placableId}");
                else
                {
                    _towers.Add(placableId,config);
                }
            }
        }

        public void SwitchCurrentId(PlacableEnum id)
        {
            if (!_towers.ContainsKey(id))
                throw new ArgumentException($"Invalid id: {id}");

            if (_towers[id]==null)
                throw new NullReferenceException($"No tower in database at id: {id}");

            _currentId = id;
        }

        public async void Build(Vector2 position, Grid grid)
        {
            if (_towers.Count==0)
                throw new Exception("There are no available towers???");

            if (_towers[_currentId]==null)
                throw new NullReferenceException($"No tower in database at id: {_currentId}");


            Vector2Int cellPosition = grid.WorldPositionToGridPosition(position);

            if (!grid.CanBuildAt(cellPosition))
                return;

            if (_waitForBuilding)
            {
                Placable inConstruction = UnityEngine.Object.Instantiate(_inConstructionPrefab, grid.GridPositionToWorldPosition(cellPosition), Quaternion.identity);
                inConstruction.Init(true);
                
                if (!grid.TryBuildAt(cellPosition, inConstruction))
                {
                    inConstruction.DestroyPlacable();
                    throw new Exception("You shouldn't be allowed to build on occupied cells, add more awailable cells checking");
                }

                Action onDestroyed = null;
                bool canceled = false;

                onDestroyed = () =>
                {
                    if (inConstruction!=null)
                    {
                        inConstruction.destroyed -= onDestroyed;
                    }
                    canceled = true;
                };

                inConstruction.destroyed+=onDestroyed;

                float timer = _towers[_currentId].BuildTime;

                while (timer>0)
                {
                    if (canceled)
                    {
                        inConstruction.DestroyPlacable();
                        inConstruction.destroyed -= onDestroyed;
                        return;
                    }

                    timer-=Time.deltaTime;

                    await Task.Delay((int)Time.deltaTime*1000);
                }


                inConstruction.DestroyPlacable();
                inConstruction.destroyed -= onDestroyed;

            }

            if (!grid.CanBuildAt(cellPosition))
                throw new Exception("Didn't you forget to delete inConstrucion placable?");

            Placable tower = UnityEngine.Object.Instantiate(_towers[_currentId].Prefab, grid.GridPositionToWorldPosition(cellPosition), Quaternion.identity);
            tower.Init(_towers[_currentId].CanBeDestroyed);

            if (!grid.TryBuildAt(cellPosition, tower))
            {
                tower.DestroyPlacable();
                throw new Exception("Didn't you forget to delete inConstrucion placable?");
            }

            tower?.OnBuild();
        }
    }
}
