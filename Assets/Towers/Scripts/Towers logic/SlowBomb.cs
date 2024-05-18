using BuffSystem;
using Enemies;
using Enemies.Buffs;
using GamePlay.EnemiesSpawning;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class SlowBomb : Bomb
    {
        private float _slowMultiplier;
        private float _slowTime;

        public void Init(SlowBombConfig config, EnemySpawner enemySpawner)
        {
            _slowMultiplier = config.SlowMultiplier;
            _slowTime = config.SlowTime;
            _spawner = enemySpawner;
            _timer = config.Delay;
            _range = config.Range;
            _damage = config.Damage;
        }

        protected override void OnEnemyDamaged(Enemy enemy)
        {
            TempBuff<EnemyStats, BuffId> tempBuff = new TempBuff<EnemyStats, BuffId>(_slowTime, _spawner, enemy, new WalkSpeedBuff(_slowMultiplier));
            enemy.AddBuff(tempBuff);
        }
    }
}
