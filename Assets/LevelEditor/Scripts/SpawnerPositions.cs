using System;
using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace LevelEditor
{
    public class SpawnerPositions
    {
        public event Action<Vector2Int> placed;
        public event Action<Vector2Int> removed;
        private List<Vector2Int> _spawners;

        public IEnumerable<Vector2Int> Spawners => _spawners;

        public SpawnerPositions()
        {
            _spawners = new List<Vector2Int>();
        }

        public bool TryAdd(Vector2Int position)
        {
            if (_spawners.Contains(position))
                return false;

            _spawners.Add(position);

            placed?.Invoke(position);

            return true;
        }
        public void Remove(Vector2Int position)
        {
            if (_spawners.Remove(position))
                removed?.Invoke(position);
        }
    }
}
