using System;
using UnityEngine;

namespace Levels.Logic
{
    [Serializable]
    public struct GridData
    {
        [SerializeField] public int gridSize;
        [SerializeField] public int[] cellsIndexes;
        [SerializeField] public int[] roadIndexes;
    }
}
