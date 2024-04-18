using System.Collections.Generic;
using Common.Interfaces;
using Enemies;
using UnityEngine;
using Waves;

namespace GamePlay
{
    public class EnemySpawner: IPausable
    {
        private readonly float _spawnRate;
        private readonly EnemyFactory _enemyFactory;
        private Wave[] _waves;
        private float _timer;
        private int _currentWaveId = 0;
        private bool _started = false;
        private bool _paused = false;
        private List<Enemy>_enemies;
        public IEnumerable<Enemy>Enemies => _enemies;
        public bool IsLastWave => _currentWaveId == _waves.Length && _waves.Length != 0;
        public EnemySpawner(Wave[] waves, float _firtsWaveDelay, float spawnRate, EnemyFactory enemyFactory)
        {
            _waves = waves;
            _timer = _firtsWaveDelay;
            _enemyFactory = enemyFactory;
            _spawnRate = spawnRate;
            _enemies = new List<Enemy>();
        }

        public void Pause()
        {
            _paused = true;

            foreach (Enemy enemy in _enemies)
            {
                enemy.Pause();
            }
        }

        public void UnPause()
        {
            foreach (Enemy enemy in _enemies)
            {
                enemy.UnPause();
            }

            _paused = false;
        }
        public void Update()
        {
            if (_paused)
                return;

            if (!_started)
                return;

            if (_timer>0)
            {
                _timer-=Time.deltaTime;
            }
            else
            {
                if (_currentWaveId>=_waves.GetLength(0))
                    return;

                _timer = _spawnRate;

                EnemyData enemyData = _waves[_currentWaveId].GetNextEnemyData();

                if (enemyData==null)
                {
                    _timer = _waves[_currentWaveId].TimeToTheNextWave;
                    _currentWaveId++;
                }
                else
                {
                    Enemy enemy = _enemyFactory.Get(enemyData.id);
                    enemy.died += OnEnemyDied;

                    if (enemy!=null)
                    {
                        _enemies.Add(enemy);

                        if (_paused)
                            enemy.Pause();
                    }
                }
            }
        }

        public void Start()
        {
            _started = true;
        }

        private void OnEnemyDied(Enemy enemy)
        {
            enemy.died -= OnEnemyDied;

            _enemies.Remove(enemy);
        }
    }
}
