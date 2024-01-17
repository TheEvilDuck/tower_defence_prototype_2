using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Waves
{
    public class Wave
    {
        private readonly float _timeToTheNextWave;
        private List<WaveEnemyData>_enemiesLeftToSpawn;

        public float TimeToTheNextWave => _timeToTheNextWave;
        
        public Wave(float timeToTheNextWave, WaveEnemyData[] enemies)
        {
            _timeToTheNextWave = timeToTheNextWave;
            _enemiesLeftToSpawn = new List<WaveEnemyData>(enemies);
        }

        public EnemyData GetNextEnemyData()
        {
            if (_enemiesLeftToSpawn.Count<=0)
                return null;

            EnemyData enemyData = _enemiesLeftToSpawn[0].enemyData;
            _enemiesLeftToSpawn[0].count--;

            if (_enemiesLeftToSpawn[0].count<=0)
                _enemiesLeftToSpawn.RemoveAt(0);

            return enemyData;


        }
    }

}