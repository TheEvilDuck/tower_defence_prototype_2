using System;
using UnityEngine;
using Waves;

namespace Levels.Logic
{
    [Serializable]
    public class LevelData
    {
        [SerializeField] public int startMoney;
        [SerializeField] public float firstWaveDelay;
        [SerializeField] public GridData gridData;
        [SerializeField] public Wave[] waves;
    }
}
