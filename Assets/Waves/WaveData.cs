using UnityEngine;
using System;

namespace Waves
{
    [Serializable]
    public class WaveData
    {
        [SerializeField]public float timeToTheNextWave;
        [SerializeField]public WaveEnemyData[] waveEnemyData;

    }
}
