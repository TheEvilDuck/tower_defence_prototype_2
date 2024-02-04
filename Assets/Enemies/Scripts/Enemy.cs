using System.Collections;
using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        private EnemyStats _baseStats;
        private LinkedList<EnemyStatsProvider> _statsModifiers;

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

        public Vector3 Position => transform.position;

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

            Debug.Log(Stats.Health);
        }
    }
}
