using System;
using UnityEngine;

namespace Levels.Logic
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField] public int gridSize;
        [SerializeField] public int startMoney;
        [SerializeField] public int[] cellsIndexes;
        [SerializeField] public int[] roadIndexes;
    }
}
