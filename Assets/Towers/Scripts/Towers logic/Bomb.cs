using BuffSystem;
using Enemies;
using Enemies.Buffs;
using GamePlay.EnemiesSpawning;
using Towers.Configs;
using UnityEngine;

namespace Towers
{
    public class Bomb : Placable
    {
        protected float _range;
        protected int _damage;
        protected float _timer;
        private bool _paused;
        protected EnemySpawner _spawner;

        private bool _inited = false;

        public void Init(BombConfig config, EnemySpawner enemySpawner)
        {
            _spawner = enemySpawner;
            _timer = config.Delay;
            _range = config.Range;
            _damage = config.Damage;
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
                        OnEnemyDamaged(enemy);
                        enemy.TakeDamage(_damage);  
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
        protected virtual void OnEnemyDamaged(Enemy enemy) {}
    }
}
