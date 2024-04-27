using System;
using UnityEngine;

namespace Levels.Logic
{
    [Serializable]
    public struct GridData
    {
        [SerializeField] public int gridSize;
        [SerializeField] public int[] cells;
        [SerializeField] public int[] roadIndexes;
    }
}
