using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyStatsProvider
    {
        private EnemyStatsProvider _wrappedEnemyStatsProvider;

        public void Wrap(EnemyStatsProvider enemyStatsProvider)
        {
            _wrappedEnemyStatsProvider = enemyStatsProvider;
        }

        public abstract EnemyStats GetStats();
    }
}
