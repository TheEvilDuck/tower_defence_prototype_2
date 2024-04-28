using BuffSystem;
using Enemies;
using Enemies.Buffs;
using GamePlay.EnemiesSpawning;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class SlowBomb : Placable
    {
        private float _range;
        private float _slowMultiplier;
        private float _slowTime;
        private float _timer;
        private bool _paused;
        private EnemySpawner _spawner;

        private bool _inited = false;

        public void Init(SlowBombConfig config, EnemySpawner enemySpawner)
        {
            _spawner = enemySpawner;
            _timer = config.Delay;
            _range = config.Range;
            _slowMultiplier = config.SlowMultiplier;
            _slowTime = config.SlowTime;
        }

        private void Update() 
        {
            if (!_inited)
                return;

            if (_paused)
                return;

            if (_timer <= 0)
            {
                foreach (Enemy enemy in _spawner.Enemies)
                {
                    if (Vector2.Distance(enemy.Position, Position) <= _range)
                    {
                        TempBuff<EnemyStats> tempBuff = new TempBuff<EnemyStats>(_slowTime, _spawner, enemy, new WalkSpeedBuff(_slowMultiplier));
                        enemy.AddBuff(tempBuff);
                    }
                }

                DestroyPlacable();
            }
            else
            {
                _timer -= Time.deltaTime;
            }
        }
        public override void Pause() => _paused = true;

        public override void UnPause() => _paused = false;

        public override void OnBuild() => _inited = true;
    }
}
