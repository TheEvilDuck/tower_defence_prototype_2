using System;
using Enemies;
using UnityEngine;

namespace Waves
{
    [Serializable]
    public class WaveEnemyData
    {
        [SerializeField] public int count;
        [SerializeField] public EnemyEnum enemyData;
    }
}
