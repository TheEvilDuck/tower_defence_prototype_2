using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using GamePlay;
using Towers.View;
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
        private EnemySpawner _spawner;

        private Enemy _target;

        private bool _initilized = false;
        private float _attackTimer = 0;

        private Vector3 Position => transform.position;

        private void Update() 
        {
            if (!_initilized!)
                return;

            FindEnemyInRange();
            HandleAttack();
        }
        public void Init(EnemySpawner spawner, TowerConfig towerConfig)
        {
            _range = towerConfig.Range;
            _damage = towerConfig.Damage;
            _attackRate = towerConfig.AttackRate;
            _spawner = spawner;
            _initilized = true;

            _attackTimer = 0;
        }


        private void FindEnemyInRange()
        {
            Enemy prevTarget = _target;

            foreach (Enemy enemy in _spawner.Enemies)
            {
                if (enemy == null)
                    continue;

                if (_target!=null)
                    if (Vector3.Distance(_target.Position, Position)>_range)
                        _target = null;

                if (Vector3.Distance(enemy.Position, Position)<=_range)
                    if (_target == null)
                        _target = enemy;
                    else if (Vector3.Distance(enemy.Position, Position)<Vector3.Distance(_target.Position, Position))
                        _target = enemy;
                        
            }

            if (prevTarget!=_target)
                targetChanged?.Invoke(_target);
        }

        private void HandleAttack()
        {
            if (_target==null)
                return;

            if (_attackTimer>=_attackRate)
            {
                _attackTimer = 0;
                _target.TakeDamage(_damage);
                attacked?.Invoke();
                return;
            }

            _attackTimer+=Time.deltaTime;
        }
    }
}
