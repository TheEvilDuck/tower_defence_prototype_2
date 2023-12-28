using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    public class EnemyStats
    {
        private int _health;
        private int _maxHealth;

        public EnemyStats(int maxHealth)
        {
            _health = _maxHealth = maxHealth;
        }
    }
}
