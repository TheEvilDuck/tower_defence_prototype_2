using System;
using UnityEngine;
using Waves;

namespace Levels.Logic
{
    [Serializable]
    public struct LevelData
    {
        public int startMoney;
        public float firstWaveDelay;
        public GridData gridData;
        public WaveData[] waves;
        public int[] spawnerPlaces;
        public PlacableData[] placables;
    }
}
