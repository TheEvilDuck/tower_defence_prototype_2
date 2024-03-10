using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Waves
{
    [Serializable]
    public class WaveEnemyData
    {
        [SerializeField] public int count;
        [SerializeField] public EnemyData enemyData;
    }
}
