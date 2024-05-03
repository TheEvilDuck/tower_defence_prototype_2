using System;
using System.Collections.Generic;
using BuffSystem;
using Builder;
using Common.Interfaces;
using Enemies.AI;
using Towers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable, IPausable, IBuffable<EnemyStats>
    {
        private const float POSITION_ACCURACY = 0.2f;
        public event Action tookDamage;
        public event Action effectApplied;
        public event Action<Enemy> died;
        private EnemyStats _baseStats;
        private EnemyStats _currentStats;
        private List<EnemyPathNode> _currentPath;
        private IDamagable _currentTarget;
        private IPlacableListHandler _placableListHandler;
        private float _lastAttackTime;
        private bool _paused = false;
        private float _pausedTime;

        public bool IsDead => _currentStats.health > 0;

        private List<IBuff<EnemyStats>> _buffs;

        public Vector2 Position => transform.position;

        public void Init(EnemyConfig enemyConfig, IPlacableListHandler placableListHandler)
        {
            _baseStats = new EnemyStats(enemyConfig);
            _currentStats = _baseStats;
            _buffs = new List<IBuff<EnemyStats>>();
            _placableListHandler = placableListHandler;
        }

        public void AddBuff(IBuff<EnemyStats> buff)
        {
            _buffs.Add(buff);
            RecalculateCurrentStats();
        }

        public void RemoveBuff(IBuff<EnemyStats> buff)
        {
            if (_buffs.Remove(buff))
                RecalculateCurrentStats();
        }

        public void Pause()
        {
            _paused = true;
            _pausedTime = Time.deltaTime;
        }

        public void UnPause()
        {
            _paused = false;
            float pausedDeltaTime = Time.time - _pausedTime;
            _lastAttackTime += pausedDeltaTime;
        }

        public void TakeDamage(int damage)
        {
            _currentStats.ModifyHealth(-damage);

            if (_currentStats.health <= 0)
                died?.Invoke(this);

            tookDamage?.Invoke();
        }

        public void UpdatePath(List<EnemyPathNode> path)
        {
            _currentPath = path;
        }

        private void FindTarget()
        {
            float distanceToNewTarget;

            foreach (Placable placable in _placableListHandler.Placables)
            {
                if (placable == null)
                    continue;

                if (placable is not IDamagable damagable)
                    continue;

                distanceToNewTarget = Vector3.Distance(placable.Position, Position);

                if (distanceToNewTarget <= _currentStats.range)
                {
                    _currentTarget = damagable;
                }
            }
        }

        private void Update() 
        {
            if (_paused)
                return;

            if (_currentStats.health <= 0)
                return;

            if (_currentPath==null)
                return;

            if (_currentPath.Count==0)
                return;

            FindTarget();

            if (_currentTarget == null)
            {
                HandleMovement();
            }
            else
            {
                HandleAttack();
            }

            
        }

        private void HandleAttack()
        {
            if (Time.time - _lastAttackTime >= _currentStats.attackRate)
            {
                _lastAttackTime = Time.time;

                _currentTarget.TakeDamage(_currentStats.damage);
            }
        }

        private void HandleMovement()
        {
            Vector2 directionVector = _currentPath[0].position-Position;

            if (directionVector.magnitude<=POSITION_ACCURACY)
            {
                _currentPath.RemoveAt(0);
                return;
            }

            transform.Translate(directionVector.normalized*_currentStats.walkSpeed*Time.deltaTime*_currentPath[0].speedMultiplier);
        }

        private void RecalculateCurrentStats()
        {
            _currentStats = _baseStats;

            foreach (var buff in _buffs)
            {
                _currentStats = buff.Apply(_currentStats);
            }
        }
    }
}
