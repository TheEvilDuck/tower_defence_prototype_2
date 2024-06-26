using System;
using Enemies;
using GamePlay.EnemiesSpawning;
using UnityEngine;

namespace Towers
{
    
    public class Tower : Placable
    {
        public event Action attacked;
        public event Action<Enemy> targetChanged;

        private float _range;
        private int _damage;
        private float _attackRate;
        protected EnemySpawner _spawner;

        protected Enemy _target;

        private bool _initilized = false;
        private float _attackTimer = 0;
        private bool _paused = false;

        private void Update() 
        {
            if (!_initilized!)
                return;

            if (_paused)
                return;

            FindEnemyInRange();
            HandleAttack();
        }
        public virtual void Init(EnemySpawner spawner, TowerConfig towerConfig)
        {
            _range = towerConfig.Range;
            _damage = towerConfig.Damage;
            _attackRate = towerConfig.AttackRate;
            _spawner = spawner;
            _initilized = true;

            _attackTimer = 0;
        }

        public override void Pause()
        {
            _paused = true;
        }

        public override void UnPause()
        {
            _paused = false;
        }


        private void FindEnemyInRange()
        {
            float distanceToTarget = float.MaxValue;
            float distanceToNewEnemy = float.MaxValue;

            if (_target != null)
            {
                distanceToTarget = Vector2.Distance(Position, _target.Position);

                if (distanceToTarget > _range)
                {
                    _target = null;
                    targetChanged?.Invoke(null);
                }

            }

            foreach (Enemy enemy in _spawner.Enemies)
            {
                distanceToNewEnemy = Vector2.Distance(Position, enemy.Position);

                if (distanceToNewEnemy > _range)
                    continue;

                if (_target == null || _target != null && !_target.IsDead)
                {
                    _target = enemy;
                    targetChanged?.Invoke(_target);
                    break;
                }

                if (distanceToNewEnemy < distanceToTarget)
                {
                    _target = enemy;
                    targetChanged?.Invoke(_target);
                    break;
                }
            }
        }

        private void HandleAttack()
        {
            if (_target==null)
                return;

            if (_attackTimer>=_attackRate)
            {
                _attackTimer = 0;
                _target.TakeDamage(_damage);
                OnAttack();
                attacked?.Invoke();
                return;
            }

            _attackTimer+=Time.deltaTime;
        }

        protected virtual void OnAttack(){}

        
    }
}
