using System;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies;
using UnityEngine;
using Waves;

namespace GamePlay.EnemiesSpawning
{
    public class EnemySpawner: IPausable, ITimer
    {
        private readonly float _spawnRate;
        private readonly EnemyFactory _enemyFactory;
        private Wave[] _waves;
        private float _timer;
        private int _currentWaveId = 0;
        private bool _started = false;
        private bool _paused = false;
        private List<Enemy>_enemies;
        private List<Enemy>_diedEnemies;

        public event Action<float> ticked;

        public IEnumerable<Enemy>Enemies => _enemies;
        public bool IsLastWave => _currentWaveId == _waves.Length && _waves.Length != 0;
        public EnemySpawner(Wave[] waves, float _firtsWaveDelay, float spawnRate, EnemyFactory enemyFactory)
        {
            _waves = waves;
            _timer = _firtsWaveDelay;
            _enemyFactory = enemyFactory;
            _spawnRate = spawnRate;
            _enemies = new List<Enemy>();
            _diedEnemies = new List<Enemy>();
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

            ticked?.Invoke(Time.deltaTime);

            if (!_started)
                return;

            foreach (Enemy enemy in _diedEnemies)
            {
                _enemies.Remove(enemy);
                UnityEngine.Object.Destroy(enemy.gameObject);
            }

            _diedEnemies.Clear();

            if (_timer>0)
            {
                _timer-=Time.deltaTime;
            }
            else
            {
                if (_currentWaveId>=_waves.GetLength(0))
                    return;

                _timer = _spawnRate;

                if (_waves[_currentWaveId].GetNextEnemyData(out EnemyEnum id))
                {
                    Enemy enemy = _enemyFactory.Get(id);
                    enemy.died += OnEnemyDied;

                    if (enemy!=null)
                    {
                        _enemies.Add(enemy);

                        if (_paused)
                            enemy.Pause();
                    }
                }
                else
                {
                    _timer = _waves[_currentWaveId].TimeToTheNextWave;
                    _currentWaveId++;
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

            _diedEnemies.Add(enemy);
        }
    }
}
