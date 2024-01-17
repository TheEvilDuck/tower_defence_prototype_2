using Enemies;
using UnityEngine;

namespace GamePlay
{
    public class EnemyFactory
    {
        private Enemy _enemyPrefab;
        private EnemyConfig _config;
        public EnemyFactory(Enemy enemyPrefab, EnemyConfig config)
        {
            _enemyPrefab = enemyPrefab;
            _config = config;
        }
        public Enemy Get()
        {
            Enemy enemy = Object.Instantiate(_enemyPrefab);
            enemy.Init(_config.MaxHealth,_config.WalkSpeed);
            return enemy;
        }
    }
}
