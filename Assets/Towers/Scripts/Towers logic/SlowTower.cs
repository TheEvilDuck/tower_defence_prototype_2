using Enemies;
using GamePlay;

namespace Towers
{
    public class SlowTower : Tower
    {
        private float _slowMultiplier;
        public override void Init(EnemySpawner spawner, TowerConfig towerConfig)
        {
            if (towerConfig is not SlowTowerConfig slowTowerConfig)
                throw new System.Exception($"You passed wrong config to slow tower");

            _slowMultiplier = slowTowerConfig.SlowMulriplier;

            base.Init(spawner, towerConfig);
        }
        protected override void OnAttack()
        {
            if (_target == null)
                return;

            //_target.AddStatsModifier(new TestSlowDecorator(_slowMultiplier));
        }
    }
}
