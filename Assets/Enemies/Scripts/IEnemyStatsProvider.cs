using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public interface IEnemyStatsProvider
    {
        public EnemyStats GetStats();
    }
}
