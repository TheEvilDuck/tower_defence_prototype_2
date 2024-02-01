using System;
using Enemies;
using UnityEngine;

namespace GamePlay
{
    public class EnemyFactory
    {
        private EnemiesDatabase _enemies;
        public EnemyFactory(EnemiesDatabase enemies)
        {
            _enemies = enemies;
        }
        public Enemy Get(EnemyEnum enemyid)
        {
            if (!_enemies.TryGetValue(enemyid, out EnemyConfig config))
                throw new ArgumentException($"There is no config for {enemyid} in enemies database");

            Enemy enemy = UnityEngine.Object.Instantiate(config.Prefab);
            enemy.Init(config.MaxHealth,config.WalkSpeed);
            return enemy;
        }
    }
}
