using System;
using BuffSystem;
using Enemies;
using Enemies.Buffs;
using GamePlay.EnemiesSpawning;
using Towers.Configs;

namespace Towers
{
    public class SlowTower : Tower
    {
        private float _slowMultiplier;
        private float _slowTime;

        public event Action<float> ticked;

        public override void Init(EnemySpawner spawner, TowerConfig towerConfig)
        {
            if (towerConfig is not SlowTowerConfig slowTowerConfig)
                throw new System.Exception($"You passed wrong config to slow tower");

            _slowMultiplier = slowTowerConfig.SlowMulriplier;
            _slowTime = slowTowerConfig.SlowTime;

            base.Init(spawner, towerConfig);
        }
        protected override void OnAttack()
        {
            if (_target == null)
                return;

            var slowBuff = new WalkSpeedBuff(_slowMultiplier);
            var tempBuff = new TempBuff<EnemyStats>(_slowTime, _spawner, _target, slowBuff);
            _target.AddBuff(tempBuff);
        }
    }
}
