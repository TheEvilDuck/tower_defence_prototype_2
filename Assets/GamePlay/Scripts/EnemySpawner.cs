using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;
using Waves;

namespace GamePlay
{
    public class EnemySpawner
    {
        private readonly float _spawnRate;
        private readonly EnemyFactory _enemyFactory;
        private Wave[] _waves;
        private float _timer;
        private int _currentWaveId = 0;
        private List<Enemy>_enemies;
        public IEnumerable<Enemy>Enemies => _enemies;
        public EnemySpawner(Wave[] waves, float _firtsWaveDelay, float spawnRate, EnemyFactory enemyFactory)
        {
            _waves = waves;
            _timer = _firtsWaveDelay;
            _enemyFactory = enemyFactory;
            _spawnRate = spawnRate;
            _enemies = new List<Enemy>();
        }
        public void Update()
        {
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
                    if (enemy!=null)
                        _enemies.Add(enemy);
                }

                
            }
        }
    }
}
