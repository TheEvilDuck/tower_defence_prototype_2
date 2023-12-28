using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class Enemy : MonoBehaviour
    {
        private EnemyStats _baseStats;
        private LinkedList<EnemyStatsProvider> _statsModifiers;

        public EnemyStats Stats
        {
            get
            {
                if (_statsModifiers.Count>0)
                    return _statsModifiers.Last.Value.GetStats();

                return _baseStats;
            }
        }

        public void Init(int maxHealth)
        {
            _baseStats = new EnemyStats(maxHealth);
            _statsModifiers = new LinkedList<EnemyStatsProvider>();
        }

        public void AddStatsModifier(EnemyStatsProvider enemyStatsProvider)
        {
            
        }
    }
}
