using System;
using System.Collections.Generic;
using Levels.Logic;
using Towers;
using UnityEngine;

namespace Builder
{
    public class PlacablesContainer : IPlacableListHandler, IDisposable
    {
        private readonly IGridCellRemovedNotifier _gridCellRemovedNotifier;
        private Dictionary<Vector2Int, Placable> _placables;
        public IEnumerable<Placable> Placables => _placables.Values;

        public PlacablesContainer(IGridCellRemovedNotifier gridCellRemovedNotifier)
        {
            _gridCellRemovedNotifier = gridCellRemovedNotifier;
            _placables = new Dictionary<Vector2Int, Placable>();

            _gridCellRemovedNotifier.cellRemoved += DestroyAt;
        }

        public void Dispose()
        {
            _gridCellRemovedNotifier.cellRemoved -= DestroyAt;
        }

        public void Pause()
        {
            foreach(var keyValuePair in _placables)
            {
                keyValuePair.Value.Pause();
            }
        }

        public void UnPause()
        {
            foreach(var keyValuePair in _placables)
            {
                keyValuePair.Value.UnPause();
            }
        }

        public bool CanBuildAt(Vector2Int position) => !_placables.ContainsKey(position);

        public bool TryBuildAt(Vector2Int position, Placable placable)
        {
            if (!CanBuildAt(position))
                return false;

            if (_placables.TryAdd(position, placable))
            {
                placable.destroyed += OnPlacableDestroyed;

                return true;
            }

            return false;
        }

        public void DestroyAt(Vector2Int position)
        {
            if (!_placables.ContainsKey(position))
                return;

            if (_placables[position].DestroyPlacable())
                _placables.Remove(position);
        }

        public void DestroyAll()
        {
            foreach (var positionAndPlacable in _placables)
            {
                positionAndPlacable.Value.destroyed -= OnPlacableDestroyed;
                positionAndPlacable.Value.ForceDestroy();
            }

            _placables.Clear();
        }

        public void ForceDestroyAt(Vector2Int position)
        {
             if (!_placables.ContainsKey(position))
                return;

            _placables[position].ForceDestroy();
                
        }

        private void OnPlacableDestroyed(Placable placable)
        {
            placable.destroyed -= OnPlacableDestroyed;

            foreach (var keyValuePair in _placables)
            {
                if (keyValuePair.Value == placable)
                {
                    _placables.Remove(keyValuePair.Key);
                    return;
                }
            }
        }
    }
}