using System;
using System.Collections.Generic;
using UnityEngine;
using Grid = Levels.Logic.Grid;

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

        public void LoadFromLevelData(int[] indexes, Grid grid)
        {
            foreach (int index in indexes)
                TryAdd(grid.ConvertIntToVector2Int(index));
        }

        public bool TryAdd(Vector2Int position)
        {
            if (_spawners.Contains(position))
                return false;

            _spawners.Add(position);

            placed?.Invoke(position);

            return true;
        }
        public bool Remove(Vector2Int position)
        {
            if (_spawners.Remove(position))
            {
                removed?.Invoke(position);
                return true;
            }

            return false;
        }
    }
}
