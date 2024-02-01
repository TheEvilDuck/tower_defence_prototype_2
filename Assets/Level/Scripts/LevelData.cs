using System;
using UnityEngine;
using Waves;

namespace Levels.Logic
{
    [Serializable]
    public struct LevelData
    {
        
        [SerializeField] public int startMoney;
        [SerializeField] public int firstWaveDelay;
        [SerializeField] public GridData gridData;
        [SerializeField] public Wave[] waves;
    }
}
