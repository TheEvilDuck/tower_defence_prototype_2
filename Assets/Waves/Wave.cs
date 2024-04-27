using System.Collections.Generic;
using Enemies;

namespace Waves
{
    public class Wave
    {
        private readonly float _timeToTheNextWave;
        private List<WaveEnemyData>_enemiesLeftToSpawn;

        public float TimeToTheNextWave => _timeToTheNextWave;
        
        public Wave(WaveData waveData)
        {
            _timeToTheNextWave = waveData.timeToTheNextWave;
            _enemiesLeftToSpawn = new List<WaveEnemyData>(waveData.waveEnemyData);
        }

        public bool GetNextEnemyData(out EnemyEnum id)
        {
            id = EnemyEnum.Gray;

            if (_enemiesLeftToSpawn.Count<=0)
                return false;

            id = _enemiesLeftToSpawn[0].enemyData;
            _enemiesLeftToSpawn[0].count--;

            if (_enemiesLeftToSpawn[0].count<=0)
                _enemiesLeftToSpawn.RemoveAt(0);

            return true;


        }
    }

}