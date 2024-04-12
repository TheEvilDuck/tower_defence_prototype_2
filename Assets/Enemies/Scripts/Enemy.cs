using System;
using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using Enemies.AI;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        private const float POSITION_ACCURACY = 0.2f;
        public event Action tookDamage;
        private EnemyStats _baseStats;
        private LinkedList<EnemyStatsProvider> _statsModifiers;
        private List<EnemyPathNode> _currentPath;

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

        public void Init(int maxHealth, float walkSpeed)
        {
            _baseStats = new EnemyStats(maxHealth, walkSpeed);
            _statsModifiers = new LinkedList<EnemyStatsProvider>();
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

            tookDamage?.Invoke();
        }

        public void UpdatePath(List<EnemyPathNode> path)
        {
            _currentPath = path;
        }

        private void Update() 
        {
            if (_currentPath==null)
                return;

            if (_currentPath.Count==0)
                return;

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
