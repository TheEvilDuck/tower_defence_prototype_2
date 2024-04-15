using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies.AI;
using Levels.Logic;
using Towers;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        private const float POSITION_ACCURACY = 0.2f;
        public event Action tookDamage;
        public event Action<Enemy> died;
        private EnemyStats _baseStats;
        private LinkedList<EnemyStatsProvider> _statsModifiers;
        private List<EnemyPathNode> _currentPath;
        private IDamagable _currentTarget;
        private IPlacableListHandler _placableListHandler;
        private float _lastAttackTime;

        public EnemyStats Stats
        {
            get
            {
                if (_statsModifiers == null)
                    throw new System.Exception("Enemy must be initialized first");

                if (_statsModifiers.Count>0)
                    return _statsModifiers.Last.Value.GetStats();

                return _baseStats;
            }
        }

        public Vector2 Position => transform.position;

        public void Init(int maxHealth, float walkSpeed, float range, float attackRate, int damage, IPlacableListHandler placableListHandler)
        {
            _baseStats = new EnemyStats(maxHealth, walkSpeed, range, attackRate, damage);
            _statsModifiers = new LinkedList<EnemyStatsProvider>();
            _placableListHandler = placableListHandler;
        }

        public void AddStatsModifier(EnemyStatsProvider enemyStatsProvider)
        {
            if (_statsModifiers.Contains(enemyStatsProvider))
                return; 

            if (_statsModifiers.Count>0)
                enemyStatsProvider.Wrap(_statsModifiers.Last.Value);
            else
                enemyStatsProvider.Wrap(_baseStats);
            
            _statsModifiers.AddLast(enemyStatsProvider);
        }

        public void RemoveStatsModifer(EnemyStatsProvider enemyStatsProvider)
        {
            if (!_statsModifiers.Contains(enemyStatsProvider))
                return;

            LinkedListNode<EnemyStatsProvider> node = _statsModifiers.Find(enemyStatsProvider);

            IEnemyStatsProvider prevProvider = _baseStats;

            if (node.Next!=null)
            {
                if (node.Previous!=null)
                    prevProvider = node.Previous.Value;

                node.Next.Value.Wrap(prevProvider);
            }

            _statsModifiers.Remove(enemyStatsProvider);
        }

        public void TakeDamage(int damage)
        {
            Stats.ModifyHealth(-damage);

            if (Stats.Health <= 0)
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

                if (distanceToNewTarget <= Stats.Range)
                {
                    _currentTarget = damagable;
                }
            }
        }

        private void Update() 
        {
            if (Stats.Health <= 0)
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
            if (Time.time - _lastAttackTime >= Stats.AttackRate)
            {
                _lastAttackTime = Time.time;

                _currentTarget.TakeDamage(Stats.Damage);
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

            transform.Translate(directionVector.normalized*Stats.WalkSpeed*Time.deltaTime*_currentPath[0].speedMultiplier);
        }
    }
}
