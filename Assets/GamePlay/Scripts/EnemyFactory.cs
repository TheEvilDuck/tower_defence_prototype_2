using System;
using Enemies;
using Enemies.AI;

namespace GamePlay
{
    public class EnemyFactory
    {
        private EnemiesDatabase _enemies;
        private PathFinder _pathFinder;
        public EnemyFactory(EnemiesDatabase enemies, PathFinder pathFinder)
        {
            _enemies = enemies;
            _pathFinder = pathFinder;
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
