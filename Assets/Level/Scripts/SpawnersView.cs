using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels.View
{
    public class SpawnersView: IDisposable
    {
        private Dictionary<Vector2Int, GameObject> _spawners;
        private readonly GameObject _spawnerPrefab;
        private readonly float _cellSize;
        private readonly int _grigSize;

        public SpawnersView(GameObject spawnerPrefab, float cellSize, int gridSize)
        {
            _spawners = new Dictionary<Vector2Int, GameObject>();
            _spawnerPrefab = spawnerPrefab;
            _cellSize = cellSize;
            _grigSize = gridSize;
        }

        public void CreateViewAt(Vector2Int position)
        {
            if (_spawners.ContainsKey(position))
                return;

            GameObject spawner = GameObject.Instantiate(_spawnerPrefab);
            Vector2 offset = new Vector2(0,0);

            if (position.x == 0)
                offset.x = -1;

            if (position.y == 0)
                offset.y = -1;

            if (position.x == _grigSize - 1)
                offset.x = 1;

            if (position.y == _grigSize - 1)
                offset.y = 1;

            spawner.transform.position = new Vector2(position.x, position.y) * _cellSize + new Vector2(_cellSize, _cellSize) / 2f + offset * _cellSize;
            _spawners.Add(position, spawner);
        }

        public void RemoveViewAt(Vector2Int position)
        {
            if (!_spawners.ContainsKey(position))
                return;

            GameObject.Destroy(_spawners[position]);
            _spawners.Remove(position);
        }

        public void Dispose()
        {
            List<Vector2Int> positions = new List<Vector2Int>(_spawners.Keys);

            while (_spawners.Count > 0)
            {
                _spawners.Remove(positions[0]);
                positions.RemoveAt(0);
            }
        }
    }
}
