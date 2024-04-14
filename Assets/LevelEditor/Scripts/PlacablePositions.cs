using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace LevelEditor
{
    public class PlacablePositions
    {
        private Dictionary<Vector2Int, PlacableEnum> _placables;

        public IReadOnlyDictionary<Vector2Int, PlacableEnum> Placables => _placables;

        public PlacablePositions()
        {
            _placables = new Dictionary<Vector2Int, PlacableEnum>();
        }

        public bool TryAdd(Vector2Int position, PlacableEnum placable) => _placables.TryAdd(position, placable);
        public void Remove(Vector2Int position) => _placables.Remove(position);
    }
}
