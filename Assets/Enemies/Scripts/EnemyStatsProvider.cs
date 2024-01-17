using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public abstract class EnemyStatsProvider: IEnemyStatsProvider
    {
        protected IEnemyStatsProvider _wrappedEnemyStatsProvider;

        public void Wrap(IEnemyStatsProvider enemyStatsProvider)
        {
            _wrappedEnemyStatsProvider = enemyStatsProvider;
        }

        public abstract EnemyStats GetStats();
    }
}
